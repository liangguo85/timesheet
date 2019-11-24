USE [master]
GO
/****** Object:  Database [ZNVTimesheet]    Script Date: 2019/11/24 12:07:12 ******/
CREATE DATABASE [ZNVTimesheet] ON  PRIMARY 
( NAME = N'ZNVTimesheet', FILENAME = N'E:\database\ZNVTimesheet.mdf' , SIZE = 9216KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ZNVTimesheet_log', FILENAME = N'E:\database\ZNVTimesheet_1.ldf' , SIZE = 10176KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ZNVTimesheet] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ZNVTimesheet].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ZNVTimesheet] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET ARITHABORT OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ZNVTimesheet] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ZNVTimesheet] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ZNVTimesheet] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ZNVTimesheet] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ZNVTimesheet] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ZNVTimesheet] SET RECOVERY FULL 
GO
ALTER DATABASE [ZNVTimesheet] SET  MULTI_USER 
GO
ALTER DATABASE [ZNVTimesheet] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ZNVTimesheet] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ZNVTimesheet', N'ON'
GO
USE [ZNVTimesheet]
GO
/****** Object:  User [liangguo]    Script Date: 2019/11/24 12:07:13 ******/
CREATE USER [liangguo] FOR LOGIN [liangguo] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [liangguo]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [liangguo]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [liangguo]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [liangguo]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [liangguo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [liangguo]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [liangguo]
GO
/****** Object:  StoredProcedure [dbo].[Proc_DepartmentReport]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[Proc_DepartmentReport]
(
	@startDate datetime,
	@endDate datetime,
	@departmentIDs nvarchar(max), -- 多个departmentID 以,号分开， 如果为空则查询所有
	@currentUserID nvarchar(50)
)
as
begin
	CREATE TABLE #AllowSearchDepartment(departmentID nvarchar(50));

	declare @searchAllDepartment bit;
	set @searchAllDepartment = 0
	-- 是否有查询所有部门的部门报表的权限
	if exists(
		select * from UserRole A
			inner join RoleModule B on A.RoleId = B.RoleId
			inner join PermissionModule C on C.Id = B.ModuleId and C.ModuleCode = '000200010001'
		where A.UserId = @currentUserID)
	begin
		set @searchAllDepartment = 1
	end

	-- 没有查询全部的权限，则查找对应的部门
	if @searchAllDepartment = 0
	begin
		-- 获取人员部门权限
		insert into #AllowSearchDepartment(departmentID) 
		select A.[DeptCode] from [MAPSysDB].[dbo].[HRDeptManager] A,[ZNVTimesheet].[dbo].[HREmployee] B 
		where A.[ManagerUserID] = B.UserID and B.EmployeeCode = @currentUserID

		-- 角色是有查询本部门报表的权限
		if exists(
			select * from UserRole A
				inner join RoleModule B on A.RoleId = B.RoleId
				inner join PermissionModule C on C.Id = B.ModuleId and C.ModuleCode = '000200010002'
			where A.UserId = @currentUserID)
		begin
			insert into #AllowSearchDepartment(departmentID) select DeptCode from [MAPSysDB].[dbo].[HREmployee] where EmployeeCode = @currentUserID
		end

		-- 角色的部门权限
		insert into #AllowSearchDepartment(departmentID) select A.DepartmentId from RoleDepartment A, UserRole B where A.RoleId = B.RoleId and B.UserId = @currentUserID
	end

	DECLARE @ColumnGroup NVARCHAR(MAX), @PivotSQL NVARCHAR(MAX);

	WITH CTE AS
	(
		SELECT    B.departmentID
		  FROM      ( SELECT    [value] = CONVERT(XML , '<v>' + REPLACE(@departmentIDs , ',' , '</v><v>')
								+ '</v>')
					) A
		  OUTER APPLY ( SELECT  departmentID = N.v.value('.' , 'nvarchar(50)')
						FROM    A.[value].nodes('/v') N ( v )
					  ) B
    )
	SELECT  D.DeptName1+ '('+D.DeptCode1+')' as '部门名称'
	  , C.EmployeeName+'('+[TimesheetUser]+')' as '人员姓名'
	  ,SUM([Workload]) AS Workload
	  ,(ISNULL(B.ProductionLineAttribute,' ') +  '--'+ B.ProjectCode + '-' + B.ProjectName) AS ProjectName
	INTO #TempTimesheet
	FROM [Timesheet] A
		INNER JOIN [Project] B ON A.ProjectID = B.Id
		INNER JOIN [MAPSysDB].[dbo].[HREmployee] C ON C.EmployeeCode = A.TimesheetUser
		INNER JOIN [MAPSysDB].[dbo].[HRDeptTree] D ON D.DeptCode1 = C.DeptCode
	WHERE A.TimesheetDate >= @startDate and A.TimesheetDate <= @endDate
		AND (
			-- 所有权限
			@searchAllDepartment = 1
			-- 部门权限
			OR EXISTS(SELECT 1 FROM #AllowSearchDepartment WHERE CHARINDEX('.'+ departmentID + '.', '.'+ D.FullDeptCode + '.') > 0)
			-- 科室权限
			OR EXISTS(SELECT 1 FROM UserSetting U INNER JOIN Team T ON U.TeamId = T.Id WHERE T.TeamLeader = @currentUserID AND U.UserId = C.EmployeeCode)
			-- 项目权限
			OR EXISTS(SELECT 1 FROM Project P WHERE P.Id = A.ProjectID AND (P.ProductLeaderID = @currentUserID OR P.ProductManagerID = @currentUserID OR P.ProjectManagerID = @currentUserID))
			-- 查询本人
			OR A.TimesheetUser = @currentUserID
		)
		AND EXISTS(SELECT 1 FROM CTE WHERE CHARINDEX('.'+ case when isnull(@departmentIDs,'') = '' then C.DeptCode else departmentID end + '.', '.'+ D.FullDeptCode + '.') > 0)
		--AND CHARINDEX(','+ C.DeptCode + ',', ','+ case when isnull(@departmentIDs,'') = '' then C.DeptCode else @departmentIDs end + ',') > 0
	GROUP BY [TimesheetUser]
      ,[ProjectID]
	  , B.ProjectCode
	  , B.ProjectName
	  , B.ProductionLineAttribute
	  , D.DeptCode1
	  , D.DeptName1
	  , C.EmployeeName
	ORDER BY D.DeptName1, C.EmployeeName

	DROP TABLE #AllowSearchDepartment

	SELECT @ColumnGroup = COALESCE(@ColumnGroup + ',' ,'' ) + QUOTENAME(ProjectName) 
	FROM #TempTimesheet
	GROUP BY QUOTENAME(ProjectName) 

	DECLARE @columnHeaders NVARCHAR (MAX)
	SELECT @columnHeaders = COALESCE(@columnHeaders + ',' ,'') + QUOTENAME(ProjectName) 
	FROM #TempTimesheet
	GROUP BY QUOTENAME(ProjectName)
	ORDER BY QUOTENAME(ProjectName)

	DECLARE @GrandTotalCol	NVARCHAR (MAX)
	SELECT @GrandTotalCol = 
	COALESCE (@GrandTotalCol + 'ISNULL('+ QUOTENAME(ProjectName) +',0) + ', 'ISNULL(' + QUOTENAME(ProjectName)+ ',0) + ')
	FROM	#TempTimesheet
	GROUP BY QUOTENAME(ProjectName)
	ORDER BY QUOTENAME(ProjectName)
	SET @GrandTotalCol = LEFT (@GrandTotalCol, LEN (@GrandTotalCol)-1)

	DECLARE @GrandTotalRow	NVARCHAR(MAX)
	SELECT @GrandTotalRow = COALESCE(@GrandTotalRow + ',ISNULL(SUM(' + QUOTENAME(ProjectName) +'),0)', 'ISNULL(SUM(' + QUOTENAME(ProjectName) +'),0)')
	FROM #TempTimesheet
	GROUP BY ProjectName
	ORDER BY ProjectName

	DECLARE @GrandSum NVARCHAR(MAX)
	SELECT @GrandSum = COALESCE(@GrandSum + ',SUM(ISNULL(' + QUOTENAME(ProjectName) +',0)) as '+QUOTENAME(ProjectName), 'SUM(ISNULL(' + QUOTENAME(ProjectName) +',0)) as '+ QUOTENAME(ProjectName))
	FROM #TempTimesheet
	GROUP BY ProjectName
	ORDER BY ProjectName

	/* MAIN QUERY */
	DECLARE @FinalQuery NVARCHAR (MAX)
	SET @FinalQuery = 	'SELECT *, ('+ @GrandTotalCol + ')
	AS [行总计] INTO #temp_MatchesTotal
				FROM
					(SELECT *
					FROM #TempTimesheet
					) A
				PIVOT
					(
					 SUM(Workload)
					 FOR ProjectName
					 IN ('+@columnHeaders +')
					) B
	ORDER BY [部门名称], [人员姓名]
	

	SELECT [部门名称],[人员姓名], '+@GrandSum+', SUM(ISNULL([行总计],0)) as [行总计(小时)] into #temp_FinalTotal FROM #temp_MatchesTotal
	group by [部门名称], [人员姓名] with rollup

	UPDATE #temp_FinalTotal SET [人员姓名] = ''部门总计(小时)'' WHERE [部门名称] IS NOT NULL AND [人员姓名] IS NULL

	UPDATE #temp_FinalTotal SET [人员姓名] = ''全部总计(小时)'', [部门名称]='''' WHERE [部门名称] IS NULL AND [人员姓名] IS NULL

	SELECT * FROM #temp_FinalTotal

	DROP TABLE #temp_MatchesTotal

	DROP TABLE #temp_FinalTotal'
	PRINT 'Pivot Query '+@FinalQuery
	EXECUTE(@FinalQuery)
	DROP TABLE #TempTimesheet
end
GO
/****** Object:  StoredProcedure [dbo].[Proc_GetNotSubmitTimesheetUserList]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--获取参数中日期所在周上班日期存在未填写工时的员工列表，发送提醒邮件
--EXEC [dbo].[Proc_GetNotSubmitTimesheetUserList] '2019-11-04,2019-11-05,2019-11-06,2019-11-07,2019-11-08'
CREATE PROCEDURE [dbo].[Proc_GetNotSubmitTimesheetUserList]
  @dateList AS nvarchar(200) --需要检查的日期列表
AS
BEGIN
	DECLARE @rowIndex INT,@maxRowIndex int
	SELECT ROW_NUMBER() OVER(ORDER BY String) AS Rownum,* into #tmpDateList FROM dbo.SPLIT(@dateList,',')
	SET @rowIndex = 1
	SELECT @maxRowIndex = max(Rownum) from #tmpDateList
	
	create TABLE #tmpResult
	(
		EmployeeCode nvarchar(50),
		EmployeeName nvarchar(50),
		[Email]        nvarchar(100)
	)
	
	DECLARE @FinalQuery NVARCHAR (MAX)
	WHILE (@rowIndex<=@maxRowIndex)
	BEGIN
		INSERT into #tmpResult(EmployeeCode,EmployeeName,[Email])
		select a.EmployeeCode,a.EmployeeName,[Email] from 
		(
			select t.* from [MAPSysDB].[dbo].[HRActiveEmployeeV] t
			left join [MAPSysDB].dbo.HRActiveDeptTreeV b on t.DeptCode = b.DeptCode1
			where b.FullDeptCode like '10000.11000%'
		) a
		left join Timesheet b on a.EmployeeCode=b.TimesheetUser 
		and SUBSTRING(CONVERT(CHAR(19), b.TimesheetDate, 120),1,10) = (SELECT String from #tmpDateList where Rownum = @rowIndex)
		where b.TimesheetDate is NULL 

		SET @rowIndex=@rowIndex+1
	END
		
	SELECT DISTINCT a.EmployeeCode,a.EmployeeName,[Email] from #tmpResult a
	--这里先只返回阿杜的账号
	where a.EmployeeCode = '0049002415'

	drop table #tmpDateList
	drop table #tmpResult
END
GO
/****** Object:  StoredProcedure [dbo].[Proc_ProductionLineReport]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[Proc_ProductionLineReport]
(
	@startDate datetime,
	@endDate datetime,
	@productionLineList nvarchar(max), -- 多个productionLine 以,号分开， 如果为空则查询所有
	@currentUserID nvarchar(50)
)
as
begin
	-- 计算工作天数(周六算工作日)
	--declare @workDays int, @workHours int
	--select @workDays = (DATEDIFF(dd, @startDate, @endDate) + 1)
	--	-(DATEDIFF(wk, @startDate, @endDate))
	--	-(case when ((DATEPART(dw, @endDate) + @@DATEFIRST) % 7) = 0 THEN 1 ELSE 0 END) -- 结束日期是星期六，减一天
	--	-(select count(*) from [Holiday] where HolidayDate between @startDate and @endDate and HolidayType = '节假日')
	--	+(select count(*) from [Holiday] where HolidayDate between @startDate and @endDate and HolidayType = '工作日')
	--set @workHours = @workDays*8

	-- 计算工作天数(周六不算工作日)
	declare @workDays int, @workHours int
	select @workDays = (DATEDIFF(dd, @startDate, @endDate) + 1)
		-(DATEDIFF(wk, @startDate, @endDate) * 2)
		-(case when ((DATEPART(dw, @startDate) + @@DATEFIRST) % 7) = 1 THEN 1 ELSE 0 END)
		-(case when ((DATEPART(dw, @endDate) + @@DATEFIRST) % 7) = 0 THEN 1 ELSE 0 END)
		-(select count(*) from [Holiday] where HolidayDate between @startDate and @endDate and HolidayType = '节假日')
		+(select count(*) from [Holiday] where HolidayDate between @startDate and @endDate and HolidayType = '工作日')
	set @workHours = @workDays*8

	CREATE TABLE #AllowSearchDepartment(departmentID nvarchar(50));

	declare @searchAllDepartment bit;
	set @searchAllDepartment = 0
	-- 是否有查询所有部门的部门报表的权限
	if exists(
		select * from UserRole A
			inner join RoleModule B on A.RoleId = B.RoleId
			inner join PermissionModule C on C.Id = B.ModuleId and C.ModuleCode = '000200040001'
		where A.UserId = @currentUserID)
	begin
		set @searchAllDepartment = 1
	end

	-- 没有查询全部的权限，则查找对应的部门
	if @searchAllDepartment = 0
	begin
		-- 获取人员部门权限
		insert into #AllowSearchDepartment(departmentID) 
		select A.[DeptCode] from [MAPSysDB].[dbo].[HRDeptManager] A,[ZNVTimesheet].[dbo].[HREmployee] B 
		where A.[ManagerUserID] = B.UserID and B.EmployeeCode = @currentUserID

		-- 角色是有查询本部门报表的权限
		if exists(
			select * from UserRole A
				inner join RoleModule B on A.RoleId = B.RoleId
				inner join PermissionModule C on C.Id = B.ModuleId and C.ModuleCode = '000200040002'
			where A.UserId = @currentUserID)
		begin
			insert into #AllowSearchDepartment(departmentID) select DeptCode from [MAPSysDB].[dbo].[HREmployee] where EmployeeCode = @currentUserID
		end

		-- 角色的部门权限
		insert into #AllowSearchDepartment(departmentID) select A.DepartmentId from RoleDepartment A, UserRole B where A.RoleId = B.RoleId and B.UserId = @currentUserID
	end

	DECLARE @ColumnGroup NVARCHAR(MAX), @PivotSQL NVARCHAR(MAX);

	SELECT  D.DeptName1+ '('+D.DeptCode1+')' as '部门名称'
	  , C.EmployeeName+'('+[TimesheetUser]+')' as '人员姓名'
	  ,cast(SUM([Workload])/@workHours as decimal(18,2)) AS Workload
	  ,(ISNULL(B.ProductionLineAttribute,' ') +  '--'+ B.ProjectCode + '-' + B.ProjectName) AS ProjectName
	INTO #TempTimesheet
	FROM [Timesheet] A
		INNER JOIN [Project] B ON A.ProjectID = B.Id
		INNER JOIN [MAPSysDB].[dbo].[HREmployee] C ON C.EmployeeCode = A.TimesheetUser
		INNER JOIN [MAPSysDB].[dbo].[HRDeptTree] D ON D.DeptCode1 = C.DeptCode
	WHERE A.TimesheetDate >= @startDate and A.TimesheetDate <= @endDate
		AND (
			-- 所有权限
			@searchAllDepartment = 1
			-- 部门权限
			OR EXISTS(SELECT 1 FROM #AllowSearchDepartment WHERE CHARINDEX('.'+ departmentID + '.', '.'+ D.FullDeptCode + '.') > 0)
			-- 科室权限
			OR EXISTS(SELECT 1 FROM UserSetting U INNER JOIN Team T ON U.TeamId = T.Id WHERE T.TeamLeader = @currentUserID AND U.UserId = C.EmployeeCode)
			-- 项目权限
			OR EXISTS(SELECT 1 FROM Project P WHERE P.Id = A.ProjectID AND (P.ProductLeaderID = @currentUserID OR P.ProductManagerID = @currentUserID OR P.ProjectManagerID = @currentUserID))
			-- 查询本人
			OR A.TimesheetUser = @currentUserID
		)
		AND CHARINDEX(','+ cast(B.ProductionLineAttribute as nvarchar(50)) + ',', ','+ case when isnull(@productionLineList, '') = '' then B.ProductionLineAttribute else @productionLineList end + ',') > 0
	GROUP BY [TimesheetUser]
      ,[ProjectID]
	  , B.ProjectCode
	  , B.ProjectName
	  , B.ProductionLineAttribute
	  , D.DeptCode1
	  , D.DeptName1
	  , C.EmployeeName
	ORDER BY D.DeptName1, C.EmployeeName

	SELECT @ColumnGroup = COALESCE(@ColumnGroup + ',' ,'' ) + QUOTENAME(ProjectName) 
	FROM #TempTimesheet
	GROUP BY QUOTENAME(ProjectName) 

	DECLARE @columnHeaders NVARCHAR (MAX)
	SELECT @columnHeaders = COALESCE(@columnHeaders + ',' ,'') + QUOTENAME(ProjectName) 
	FROM #TempTimesheet
	GROUP BY QUOTENAME(ProjectName)
	ORDER BY QUOTENAME(ProjectName)

	DECLARE @GrandTotalCol	NVARCHAR (MAX)
	SELECT @GrandTotalCol = 
	COALESCE (@GrandTotalCol + 'ISNULL ('+ QUOTENAME(ProjectName) +',0) + ', 'ISNULL(' + QUOTENAME(ProjectName)+ ',0) + ')
	FROM	#TempTimesheet
	GROUP BY QUOTENAME(ProjectName)
	ORDER BY QUOTENAME(ProjectName)
	SET @GrandTotalCol = LEFT (@GrandTotalCol, LEN (@GrandTotalCol)-1)

	DECLARE @GrandTotalRow	NVARCHAR(MAX)
	SELECT @GrandTotalRow = COALESCE(@GrandTotalRow + ',ISNULL(SUM(' + QUOTENAME(ProjectName) +'),0)', 'ISNULL(SUM(' + QUOTENAME(ProjectName) +'),0)')
	FROM #TempTimesheet
	GROUP BY ProjectName
	ORDER BY ProjectName

	DECLARE @GrandSum NVARCHAR(MAX)
	SELECT @GrandSum = COALESCE(@GrandSum + ',SUM(ISNULL(' + QUOTENAME(ProjectName) +',0)) as '+QUOTENAME(ProjectName), 'SUM(ISNULL(' + QUOTENAME(ProjectName) +',0)) as '+ QUOTENAME(ProjectName))
	FROM #TempTimesheet
	GROUP BY ProjectName
	ORDER BY ProjectName

	/* MAIN QUERY */
	DECLARE @FinalQuery NVARCHAR (MAX)
	SET @FinalQuery = 	'SELECT *, ('+ @GrandTotalCol + ')
	AS [行总计] INTO #temp_MatchesTotal
				FROM
					(SELECT *
					FROM #TempTimesheet
					) A
				PIVOT
					(
					 SUM(Workload)
					 FOR ProjectName
					 IN ('+@columnHeaders +')
					) B
	ORDER BY [部门名称], [人员姓名]
	

	SELECT [部门名称],[人员姓名], '+@GrandSum+', SUM(ISNULL([行总计],0)) as [行总计(人力)] into #temp_FinalTotal FROM #temp_MatchesTotal
	group by [部门名称], [人员姓名] with rollup

	UPDATE #temp_FinalTotal SET [人员姓名] = ''部门总计(人力)'' WHERE [部门名称] IS NOT NULL AND [人员姓名] IS NULL

	UPDATE #temp_FinalTotal SET [人员姓名] = ''全部总计(人力)'', [部门名称]='''' WHERE [部门名称] IS NULL AND [人员姓名] IS NULL

	SELECT * FROM #temp_FinalTotal

	DROP TABLE #temp_MatchesTotal

	DROP TABLE #temp_FinalTotal'
	PRINT 'Pivot Query '+@FinalQuery
	EXECUTE(@FinalQuery)
	DROP TABLE #AllowSearchDepartment
	DROP TABLE #TempTimesheet
end
GO
/****** Object:  StoredProcedure [dbo].[Proc_ProjectManpowerReport]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[Proc_ProjectManpowerReport]
(
	@startDate datetime,
	@endDate datetime,
	@projectIDs nvarchar(max), -- 多个projectID 以,号分开， 如果为空则查询所有
	@currentUserID nvarchar(50)
)
as
begin
	-- 计算工作天数(目前周六算工作日)
	--declare @workDays int, @workHours int
	--select @workDays = (DATEDIFF(dd, @startDate, @endDate) + 1)
	--	-(DATEDIFF(wk, @startDate, @endDate))
	--	-(case when ((DATEPART(dw, @endDate) + @@DATEFIRST) % 7) = 0 THEN 1 ELSE 0 END) -- 结束日期是星期六，减一天
	--	-(select count(*) from [Holiday] where HolidayDate between @startDate and @endDate and HolidayType = '节假日')
	--	+(select count(*) from [Holiday] where HolidayDate between @startDate and @endDate and HolidayType = '工作日')
	--set @workHours = @workDays*8

	-- 计算工作天数(目前周六不算工作日)
	declare @workDays int, @workHours int
	select @workDays = (DATEDIFF(dd, @startDate, @endDate) + 1)
		-(DATEDIFF(wk, @startDate, @endDate) * 2)
		-(case when ((DATEPART(dw, @startDate) + @@DATEFIRST) % 7) = 1 THEN 1 ELSE 0 END)
		-(case when ((DATEPART(dw, @endDate) + @@DATEFIRST) % 7) = 0 THEN 1 ELSE 0 END)
		-(select count(*) from [Holiday] where HolidayDate between @startDate and @endDate and HolidayType = '节假日')
		+(select count(*) from [Holiday] where HolidayDate between @startDate and @endDate and HolidayType = '工作日')
	set @workHours = @workDays*8

	CREATE TABLE #AllowSearchDepartment(departmentID nvarchar(50));

	declare @searchAllDepartment bit;
	set @searchAllDepartment = 0
	-- 是否有查询所有部门的部门报表的权限
	if exists(
		select * from UserRole A
			inner join RoleModule B on A.RoleId = B.RoleId
			inner join PermissionModule C on C.Id = B.ModuleId and C.ModuleCode = '000200030001'
		where A.UserId = @currentUserID)
	begin
		set @searchAllDepartment = 1
	end

	-- 没有查询全部的权限，则查找对应的部门
	if @searchAllDepartment = 0
	begin
		-- 获取人员部门权限
		insert into #AllowSearchDepartment(departmentID) 
		select A.[DeptCode] from [MAPSysDB].[dbo].[HRDeptManager] A,[ZNVTimesheet].[dbo].[HREmployee] B 
		where A.[ManagerUserID] = B.UserID and B.EmployeeCode = @currentUserID

		-- 角色是有查询本部门报表的权限
		if exists(
			select * from UserRole A
				inner join RoleModule B on A.RoleId = B.RoleId
				inner join PermissionModule C on C.Id = B.ModuleId and C.ModuleCode = '000200030002'
			where A.UserId = @currentUserID)
		begin
			insert into #AllowSearchDepartment(departmentID) select DeptCode from [MAPSysDB].[dbo].[HREmployee] where EmployeeCode = @currentUserID
		end

		-- 角色的部门权限
		insert into #AllowSearchDepartment(departmentID) select A.DepartmentId from RoleDepartment A, UserRole B where A.RoleId = B.RoleId and B.UserId = @currentUserID
	end

	DECLARE @ColumnGroup NVARCHAR(MAX), @PivotSQL NVARCHAR(MAX);

	SELECT  D.DeptName1+ '('+D.DeptCode1+')' as '部门名称'
	  , C.EmployeeName+'('+[TimesheetUser]+')' as '人员姓名'
	  ,cast(SUM([Workload])/@workHours as decimal(18,2)) AS Workload
	  ,(ISNULL(B.ProductionLineAttribute,' ') +  '--'+ B.ProjectCode + '-'  + B.ProjectName) AS ProjectName
	INTO #TempTimesheet
	FROM [Timesheet] A
		INNER JOIN [Project] B ON A.ProjectID = B.Id
		INNER JOIN [MAPSysDB].[dbo].[HREmployee] C ON C.EmployeeCode = A.TimesheetUser
		INNER JOIN [MAPSysDB].[dbo].[HRDeptTree] D ON D.DeptCode1 = C.DeptCode
	WHERE A.TimesheetDate >= @startDate and A.TimesheetDate <= @endDate
		AND (
			-- 所有权限
			@searchAllDepartment = 1
			-- 部门权限
			OR EXISTS(SELECT 1 FROM #AllowSearchDepartment WHERE CHARINDEX('.'+ departmentID + '.', '.'+ D.FullDeptCode + '.') > 0)
			-- 科室权限
			OR EXISTS(SELECT 1 FROM UserSetting U INNER JOIN Team T ON U.TeamId = T.Id WHERE T.TeamLeader = @currentUserID AND U.UserId = C.EmployeeCode)
			-- 项目权限
			OR EXISTS(SELECT 1 FROM Project P WHERE P.Id = A.ProjectID AND (P.ProductLeaderID = @currentUserID OR P.ProductManagerID = @currentUserID OR P.ProjectManagerID = @currentUserID))
			-- 查询本人
			OR A.TimesheetUser = @currentUserID
		)
		AND CHARINDEX(','+ cast(A.ProjectID as nvarchar(50)) + ',', ','+ case when isnull(@projectIDs, '') = '' then cast(A.ProjectID as nvarchar(50)) else @projectIDs end + ',') > 0
	GROUP BY [TimesheetUser]
      ,[ProjectID]
	  , B.ProjectCode
	  , B.ProjectName
	  , B.ProductionLineAttribute
	  , D.DeptCode1
	  , D.DeptName1
	  , C.EmployeeName
	ORDER BY D.DeptName1, C.EmployeeName

	SELECT @ColumnGroup = COALESCE(@ColumnGroup + ',' ,'' ) + QUOTENAME(ProjectName) 
	FROM #TempTimesheet
	GROUP BY QUOTENAME(ProjectName) 

	DECLARE @columnHeaders NVARCHAR (MAX)
	SELECT @columnHeaders = COALESCE(@columnHeaders + ',' ,'') + QUOTENAME(ProjectName) 
	FROM #TempTimesheet
	GROUP BY QUOTENAME(ProjectName)
	ORDER BY QUOTENAME(ProjectName)

	DECLARE @GrandTotalCol	NVARCHAR (MAX)
	SELECT @GrandTotalCol = 
	COALESCE (@GrandTotalCol + 'ISNULL ('+ QUOTENAME(ProjectName) +',0) + ', 'ISNULL(' + QUOTENAME(ProjectName)+ ',0) + ')
	FROM	#TempTimesheet
	GROUP BY QUOTENAME(ProjectName)
	ORDER BY QUOTENAME(ProjectName)
	SET @GrandTotalCol = LEFT (@GrandTotalCol, LEN (@GrandTotalCol)-1)

	DECLARE @GrandTotalRow	NVARCHAR(MAX)
	SELECT @GrandTotalRow = COALESCE(@GrandTotalRow + ',ISNULL(SUM(' + QUOTENAME(ProjectName) +'),0)', 'ISNULL(SUM(' + QUOTENAME(ProjectName) +'),0)')
	FROM #TempTimesheet
	GROUP BY ProjectName
	ORDER BY ProjectName

	DECLARE @GrandSum NVARCHAR(MAX)
	SELECT @GrandSum = COALESCE(@GrandSum + ',SUM(ISNULL(' + QUOTENAME(ProjectName) +',0)) as '+QUOTENAME(ProjectName), 'SUM(ISNULL(' + QUOTENAME(ProjectName) +',0)) as '+ QUOTENAME(ProjectName))
	FROM #TempTimesheet
	GROUP BY ProjectName
	ORDER BY ProjectName

	/* MAIN QUERY */
	DECLARE @FinalQuery NVARCHAR (MAX)
	SET @FinalQuery = 	'SELECT *, ('+ @GrandTotalCol + ')
	AS [行总计] INTO #temp_MatchesTotal
				FROM
					(SELECT *
					FROM #TempTimesheet
					) A
				PIVOT
					(
					 SUM(Workload)
					 FOR ProjectName
					 IN ('+@columnHeaders +')
					) B
	ORDER BY [部门名称], [人员姓名]
	

	SELECT [部门名称],[人员姓名], '+@GrandSum+', SUM(ISNULL([行总计],0)) as [行总计(人力)] into #temp_FinalTotal FROM #temp_MatchesTotal
	group by [部门名称], [人员姓名] with rollup

	UPDATE #temp_FinalTotal SET [人员姓名] = ''部门总计(人力)'' WHERE [部门名称] IS NOT NULL AND [人员姓名] IS NULL

	UPDATE #temp_FinalTotal SET [人员姓名] = ''全部总计(人力)'', [部门名称]='''' WHERE [部门名称] IS NULL AND [人员姓名] IS NULL

	SELECT * FROM #temp_FinalTotal

	DROP TABLE #temp_MatchesTotal

	DROP TABLE #temp_FinalTotal'
	PRINT 'Pivot Query '+@FinalQuery
	EXECUTE(@FinalQuery)
	DROP TABLE #AllowSearchDepartment
	DROP TABLE #TempTimesheet
end
GO
/****** Object:  StoredProcedure [dbo].[Proc_ProjectReport]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[Proc_ProjectReport]
(
	@startDate datetime,
	@endDate datetime,
	@projectIDs nvarchar(max), -- 多个projectID 以,号分开， 如果为空则查询所有
	@currentUserID nvarchar(50)
)
as
begin
	CREATE TABLE #AllowSearchDepartment(departmentID nvarchar(50));

	declare @searchAllDepartment bit;
	set @searchAllDepartment = 0
	-- 是否有查询所有部门的部门报表的权限
	if exists(
		select * from UserRole A
			inner join RoleModule B on A.RoleId = B.RoleId
			inner join PermissionModule C on C.Id = B.ModuleId and C.ModuleCode = '000200020001'
		where A.UserId = @currentUserID)
	begin
		set @searchAllDepartment = 1
	end

	-- 没有查询全部的权限，则查找对应的部门
	if @searchAllDepartment = 0
	begin
		-- 获取人员部门权限
		insert into #AllowSearchDepartment(departmentID) 
		select A.[DeptCode] from [MAPSysDB].[dbo].[HRDeptManager] A,[ZNVTimesheet].[dbo].[HREmployee] B 
		where A.[ManagerUserID] = B.UserID and B.EmployeeCode = @currentUserID

		-- 角色是有查询本部门报表的权限
		if exists(
			select * from UserRole A
				inner join RoleModule B on A.RoleId = B.RoleId
				inner join PermissionModule C on C.Id = B.ModuleId and C.ModuleCode = '000200020002'
			where A.UserId = @currentUserID)
		begin
			insert into #AllowSearchDepartment(departmentID) select DeptCode from [MAPSysDB].[dbo].[HREmployee] where EmployeeCode = @currentUserID
		end

		-- 角色的部门权限
		insert into #AllowSearchDepartment(departmentID) select A.DepartmentId from RoleDepartment A, UserRole B where A.RoleId = B.RoleId and B.UserId = @currentUserID
	end

	DECLARE @ColumnGroup NVARCHAR(MAX), @PivotSQL NVARCHAR(MAX);

	SELECT  D.DeptName1+ '('+D.DeptCode1+')' as '部门名称'
	  , [TimesheetUser] as '工号'
	  , C.EmployeeName as '姓名'
	  ,SUM([Workload]) AS Workload
	  ,(ISNULL(B.ProductionLineAttribute,' ') +  '--' + B.ProjectCode + '-' + B.ProjectName) AS ProjectName
	INTO #TempTimesheet
	FROM [Timesheet] A
		INNER JOIN [Project] B ON A.ProjectID = B.Id
		INNER JOIN [MAPSysDB].[dbo].[HREmployee] C ON C.EmployeeCode = A.TimesheetUser
		INNER JOIN [MAPSysDB].[dbo].[HRDeptTree] D ON D.DeptCode1 = C.DeptCode
	WHERE A.TimesheetDate >= @startDate and A.TimesheetDate <= @endDate
		AND (
			-- 所有权限
			@searchAllDepartment = 1
			-- 部门权限
			OR EXISTS(SELECT 1 FROM #AllowSearchDepartment WHERE CHARINDEX('.'+ departmentID + '.', '.'+ D.FullDeptCode + '.') > 0)
			-- 科室权限
			OR EXISTS(SELECT 1 FROM UserSetting U INNER JOIN Team T ON U.TeamId = T.Id WHERE T.TeamLeader = @currentUserID AND U.UserId = C.EmployeeCode)
			-- 项目权限
			OR EXISTS(SELECT 1 FROM Project P WHERE P.Id = A.ProjectID AND (P.ProductLeaderID = @currentUserID OR P.ProductManagerID = @currentUserID OR P.ProjectManagerID = @currentUserID))
			-- 查询本人
			OR A.TimesheetUser = @currentUserID
		)
		AND CHARINDEX(','+ cast(A.ProjectID as nvarchar(50)) + ',', ','+ case when isnull(@projectIDs, '') = '' then cast(A.ProjectID as nvarchar(50)) else @projectIDs end + ',') > 0
	GROUP BY [TimesheetUser]
      ,[ProjectID]
	  , B.ProjectCode
	  , B.ProjectName
	  , B.ProductionLineAttribute
	  , D.DeptCode1
	  , D.DeptName1
	  , C.EmployeeName
	ORDER BY D.DeptName1, C.EmployeeName

	SELECT @ColumnGroup = COALESCE(@ColumnGroup + ',' ,'' ) + QUOTENAME(ProjectName) 
	FROM #TempTimesheet
	GROUP BY QUOTENAME(ProjectName) 

	DECLARE @columnHeaders NVARCHAR (MAX)
	SELECT @columnHeaders = COALESCE(@columnHeaders + ',' ,'') + QUOTENAME(ProjectName) 
	FROM #TempTimesheet
	GROUP BY QUOTENAME(ProjectName)
	ORDER BY QUOTENAME(ProjectName)

	DECLARE @GrandTotalCol	NVARCHAR (MAX)
	SELECT @GrandTotalCol = 
	COALESCE (@GrandTotalCol + 'ISNULL ('+ QUOTENAME(ProjectName) +',0) + ', 'ISNULL(' + QUOTENAME(ProjectName)+ ',0) + ')
	FROM	#TempTimesheet
	GROUP BY QUOTENAME(ProjectName)
	ORDER BY QUOTENAME(ProjectName)
	SET @GrandTotalCol = LEFT (@GrandTotalCol, LEN (@GrandTotalCol)-1)

	DECLARE @GrandTotalRow	NVARCHAR(MAX)
	SELECT @GrandTotalRow = COALESCE(@GrandTotalRow + ',ISNULL(SUM(' + QUOTENAME(ProjectName) +'),0)', 'ISNULL(SUM(' + QUOTENAME(ProjectName) +'),0)')
	FROM #TempTimesheet
	GROUP BY ProjectName
	ORDER BY ProjectName

	DECLARE @GrandSum NVARCHAR(MAX)
	SELECT @GrandSum = COALESCE(@GrandSum + ',SUM(ISNULL(' + QUOTENAME(ProjectName) +',0)) as '+QUOTENAME(ProjectName), 'SUM(ISNULL(' + QUOTENAME(ProjectName) +',0)) as '+ QUOTENAME(ProjectName))
	FROM #TempTimesheet
	GROUP BY ProjectName
	ORDER BY ProjectName

	/* MAIN QUERY */
	DECLARE @FinalQuery NVARCHAR (MAX)
	SET @FinalQuery = 	'SELECT *, ('+ @GrandTotalCol + ')
	AS [行总计] INTO #temp_MatchesTotal
				FROM
					(SELECT *
					FROM #TempTimesheet
					) A
				PIVOT
					(
					 SUM(Workload)
					 FOR ProjectName
					 IN ('+@columnHeaders +')
					) B
	ORDER BY [部门名称], [姓名]
	

	SELECT [部门名称],[工号],[姓名],'+@GrandSum+', SUM(ISNULL([行总计],0)) as [行总计(小时)] into #temp_FinalTotal FROM #temp_MatchesTotal
	group by [部门名称], [姓名], [工号] with rollup

	DELETE FROM #temp_FinalTotal WHERE [部门名称] IS NOT NULL AND ([姓名] IS NULL OR [工号] IS NULL)

	UPDATE #temp_FinalTotal SET [姓名] = ''全部总计(小时)'', [部门名称]='''', [工号]='''' WHERE [部门名称] IS NULL AND [姓名] IS NULL

	SELECT * FROM #temp_FinalTotal

	DROP TABLE #temp_MatchesTotal

	DROP TABLE #temp_FinalTotal'
	PRINT 'Pivot Query '+@FinalQuery
	EXECUTE(@FinalQuery)
	DROP TABLE #AllowSearchDepartment
	DROP TABLE #TempTimesheet
end
GO
/****** Object:  StoredProcedure [dbo].[Proc_TimesheetReport]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[Proc_TimesheetReport]
(
	@startDate datetime,
	@endDate datetime,
	@departmentIDs nvarchar(max), -- 多个departmentID 以,号分开， 如果为空则查询所有
	@productionLineList nvarchar(max),
	@projectIds nvarchar(max),
	@userIds nvarchar(max),
	@currentUserID nvarchar(50),
	@isPage bit,-- 是否分页【解决导出Excel全部导出问题】
	@pageSize int, -- 每页记录数
	@pageStart int,
	@totalRecords int output -- 总记录数
)
as
begin
	CREATE TABLE #AllowSearchDepartment(departmentID nvarchar(50));

	declare @searchAllDepartment bit;
	set @searchAllDepartment = 0
	-- 是否有查询所有部门的部门报表的权限
	if exists(
		select * from UserRole A
			inner join RoleModule B on A.RoleId = B.RoleId
			inner join PermissionModule C on C.Id = B.ModuleId and C.ModuleCode = '000200050001'
		where A.UserId = @currentUserID)
	begin
		set @searchAllDepartment = 1
	end

	-- 没有查询全部的权限，则查找对应的部门
	if @searchAllDepartment = 0
	begin
		-- 获取人员部门权限
		insert into #AllowSearchDepartment(departmentID) 
		select A.[DeptCode] from [MAPSysDB].[dbo].[HRDeptManager] A,[ZNVTimesheet].[dbo].[HREmployee] B 
		where A.[ManagerUserID] = B.UserID and B.EmployeeCode = @currentUserID

		-- 角色是有查询本部门报表的权限
		if exists(
			select * from UserRole A
				inner join RoleModule B on A.RoleId = B.RoleId
				inner join PermissionModule C on C.Id = B.ModuleId and C.ModuleCode = '000200050002'
			where A.UserId = @currentUserID)
		begin
			insert into #AllowSearchDepartment(departmentID) select DeptCode from [MAPSysDB].[dbo].[HREmployee] where EmployeeCode = @currentUserID
		end

		-- 角色的部门权限
		insert into #AllowSearchDepartment(departmentID) select A.DepartmentId from RoleDepartment A, UserRole B where A.RoleId = B.RoleId and B.UserId = @currentUserID
	end

	DECLARE @ColumnGroup NVARCHAR(MAX), @PivotSQL NVARCHAR(MAX);

	WITH CTE AS
	(
		SELECT    B.departmentID
		  FROM      ( SELECT    [value] = CONVERT(XML , '<v>' + REPLACE(@departmentIDs , ',' , '</v><v>')
								+ '</v>')
					) A
		  OUTER APPLY ( SELECT  departmentID = N.v.value('.' , 'nvarchar(50)')
						FROM    A.[value].nodes('/v') N ( v )
					  ) B
    )
	SELECT  RowID = ROW_NUMBER() over(order by D.DeptName1, C.EmployeeName, B.ProjectName, A.TimesheetDate desc),
		D.DeptName1 as 'DeptName'
	  , C.EmployeeName+'('+A.TimesheetUser+')' as 'EmployeeName'
	  , B.ProjectCode
	  , B.ProjectName
	  ,ISNULL(B.ProductionLineAttribute,'') as 'ProductionLineAttribute'
	  ,ISNULL(E.EmployeeName,' ') as 'ProductManager'
	  ,ISNULL(F.EmployeeName,' ') as 'ProjectManager'
	  , A.TimesheetDate
	  ,[Workload]
	  ,case A.Status when 0 then '草稿' when 1 then '审核中' when 2 then '审批通过' when 3 then '驳回' else '' end as 'Status'
	  ,WorkContent
	  ,Remarks
	INTO #TempTimesheet
	FROM [Timesheet] A
		INNER JOIN [Project] B ON A.ProjectID = B.Id
		INNER JOIN [MAPSysDB].[dbo].[HREmployee] C ON C.EmployeeCode = A.TimesheetUser
		INNER JOIN [MAPSysDB].[dbo].[HRDeptTree] D ON D.DeptCode1 = C.DeptCode
		LEFT JOIN [MAPSysDB].[dbo].[HREmployee] E ON E.EmployeeCode = B.ProductManagerID
		LEFT JOIN [MAPSysDB].[dbo].[HREmployee] F ON F.EmployeeCode = B.ProjectManagerID
	WHERE A.TimesheetDate >= @startDate and A.TimesheetDate <= @endDate
		AND (
			-- 所有权限
			@searchAllDepartment = 1
			-- 部门权限
			OR EXISTS(SELECT 1 FROM #AllowSearchDepartment WHERE CHARINDEX('.'+ departmentID + '.', '.'+ D.FullDeptCode + '.') > 0)
			-- 科室权限
			OR EXISTS(SELECT 1 FROM UserSetting U INNER JOIN Team T ON U.TeamId = T.Id WHERE T.TeamLeader = @currentUserID AND U.UserId = C.EmployeeCode)
			-- 项目权限
			OR EXISTS(SELECT 1 FROM Project P WHERE P.Id = A.ProjectID AND (P.ProductLeaderID = @currentUserID OR P.ProductManagerID = @currentUserID OR P.ProjectManagerID = @currentUserID))
			-- 查询本人
			OR A.TimesheetUser = @currentUserID
		)
		AND EXISTS(SELECT 1 FROM CTE WHERE CHARINDEX('.'+ case when isnull(@departmentIDs,'') = '' then C.DeptCode else departmentID end + '.', '.'+ D.FullDeptCode + '.') > 0)
		AND CHARINDEX(','+ cast(A.ProjectID as nvarchar(50)) + ',', ','+ case when isnull(@projectIDs, '') = '' then cast(A.ProjectID as nvarchar(50)) else @projectIDs end + ',') > 0
		AND CHARINDEX(','+ cast(B.ProductionLineAttribute as nvarchar(50)) + ',', ','+ case when isnull(@productionLineList, '') = '' then B.ProductionLineAttribute else @productionLineList end + ',') > 0
		AND CHARINDEX(','+ cast(A.TimesheetUser as nvarchar(50)) + ',', ','+ case when isnull(@userIds, '') = '' then A.TimesheetUser else @userIds end + ',') > 0

	-- 是否分页
	select @TotalRecords = COUNT(*) from #TempTimesheet -- 设置总记录数
	if(@isPage = 1)
	begin
		delete A from #TempTimesheet A
		where not exists(
		select 1 from #TempTimesheet
		where RowID > @pageStart
			and RowID <= @pageStart+@PageSize
			and A.RowID = RowID)
	end
	select DeptName as '部门'
	  , EmployeeName as '员工'
	  , ProjectCode as '项目编码'
	  , ProjectName as '项目名称'
	  , ProductionLineAttribute as '产品线'
	  , ProductManager as '产品经理'
	  , ProjectManager as '项目经理'
	  , CONVERT(char(10), TimesheetDate,120) as '日期'
	  ,[Workload] as '工时'
	  ,[Status] as '状态'
	  ,WorkContent as '工作内容'
	  ,Remarks as '备注'
	from #TempTimesheet

	DROP TABLE #AllowSearchDepartment
	DROP TABLE #TempTimesheet
end
GO
/****** Object:  UserDefinedFunction [dbo].[SPLIT]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SPLIT] 
(
		@string nvarchar(500),  
    @separator nvarchar(10) 
)
RETURNS @array TABLE(String nvarchar(500)) 
AS
BEGIN
	-- Declare the return variable here
	DECLARE @separatorIndex int,@tempString nvarchar(500),@tagString nvarchar(500)
	-- Add the T-SQL statements to compute the return value here
  SET @tagString=@string
	SET @separatorIndex=CHARINDEX(@separator,@tagString)  

  WHILE(@separatorIndex<>0)
	BEGIN
		SET @tempString = SUBSTRING(@tagString,1,@separatorIndex-1)  
		INSERT INTO @array(String) VALUES(@tempString)  
		SET @tagString = SUBSTRING(@tagString,@separatorIndex+1,LEN(@tagString)-@separatorIndex)  
		SET @separatorIndex=CHARINDEX(@separator,@tagString)  
	END  

	SET @tempString = @tagString  
	IF (LEN(@tempString)>0)  
			INSERT INTO @array(String) VALUES(@tagString)  
	-- Return the result of the function
	RETURN
END

GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ApproveLog]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApproveLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkflowInstanceID] [nvarchar](50) NOT NULL,
	[CurrentOperator] [nvarchar](50) NOT NULL,
	[NextOperator] [nvarchar](50) NULL,
	[Comment] [nvarchar](50) NOT NULL,
	[OperateType] [nvarchar](20) NOT NULL,
	[OperateTime] [datetime] NOT NULL,
	[Creator] [nvarchar](50) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](50) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ApproveLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Configuration]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configuration](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ConfigValue] [nvarchar](50) NOT NULL,
	[ConfigText] [nvarchar](50) NOT NULL,
	[ParentId] [int] NULL,
	[Level] [int] NOT NULL,
	[Creator] [nvarchar](20) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmailTemplate]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmailTemplateCode] [nvarchar](50) NOT NULL,
	[EmailTemplateName] [nvarchar](500) NULL,
	[EmailTemplateBody] [nvarchar](max) NOT NULL,
	[Creator] [nvarchar](20) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Holiday]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Holiday](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HolidayDate] [datetime] NULL,
	[HolidayType] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](max) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Holiday] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HRDeptManager]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HRDeptManager](
	[DeptCode] [nvarchar](100) NOT NULL,
	[ManagerUserID] [nvarchar](100) NOT NULL,
	[ActiveDate] [datetime] NOT NULL,
	[InactiveDate] [datetime] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[LastUpdateDate] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NOT NULL,
	[LastUpdateLogin] [nvarchar](100) NULL,
 CONSTRAINT [HRDeptManager_PK] PRIMARY KEY CLUSTERED 
(
	[DeptCode] ASC,
	[ManagerUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HRDeptTree]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HRDeptTree](
	[FullDeptCode] [nvarchar](100) NOT NULL,
	[FullDeptName] [nvarchar](250) NOT NULL,
	[IsActiveDept] [nvarchar](1) NOT NULL,
	[DeptCode1] [nvarchar](100) NOT NULL,
	[DeptName1] [nvarchar](100) NOT NULL,
	[DeptLay1] [int] NOT NULL,
	[DeptCode2] [nvarchar](100) NULL,
	[DeptName2] [nvarchar](100) NULL,
	[DeptLay2] [int] NULL,
	[DeptCode3] [nvarchar](100) NULL,
	[DeptName3] [nvarchar](100) NULL,
	[DeptLay3] [int] NULL,
	[DeptCode4] [nvarchar](100) NULL,
	[DeptName4] [nvarchar](100) NULL,
	[DeptLay4] [int] NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[LastUpdateDate] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NOT NULL,
	[LastUpdateLogin] [nvarchar](100) NULL,
 CONSTRAINT [HRDeptTree_PK] PRIMARY KEY CLUSTERED 
(
	[FullDeptCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HREmployee]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HREmployee](
	[UserID] [nvarchar](100) NOT NULL,
	[EmployeeCode] [nvarchar](100) NOT NULL,
	[EmployeeName] [nvarchar](100) NOT NULL,
	[EntryDate] [datetime] NULL,
	[ExitDate] [datetime] NULL,
	[Gender] [nvarchar](10) NOT NULL,
	[SocialID] [nvarchar](100) NOT NULL,
	[MobilePhone] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PYName] [nvarchar](250) NOT NULL,
	[OrgCode] [nvarchar](100) NOT NULL,
	[DeptCode] [nvarchar](100) NULL,
	[ResidencePlaceCode] [nvarchar](100) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[LastUpdateDate] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NOT NULL,
	[LastUpdateLogin] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PermissionModule]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionModule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleCode] [nvarchar](50) NULL,
	[ModuleName] [nvarchar](50) NOT NULL,
	[ParentModuleId] [int] NULL,
	[Level] [int] NOT NULL,
	[Creator] [nvarchar](50) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](50) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_PermissionModule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Project]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](50) NULL,
	[ProjectCode] [nvarchar](50) NOT NULL,
	[ProjectName] [nvarchar](50) NOT NULL,
	[ProjectManagerID] [nvarchar](20) NOT NULL,
	[ProductManagerID] [nvarchar](20) NOT NULL,
	[ProductLeaderID] [nvarchar](50) NULL,
	[ProjectType] [nvarchar](10) NULL,
	[ProjectLevel] [nvarchar](10) NULL,
	[ProjectKind] [nvarchar](10) NULL,
	[ProductionLineAttribute] [nvarchar](20) NULL,
	[ProjectStatus] [nvarchar](20) NULL,
	[IsEnabled] [bit] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[ExpirationDate] [datetime] NOT NULL,
	[Creator] [nvarchar](20) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Project_copy]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project_copy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsApproval] [bit] NOT NULL,
	[ProjectCode] [nvarchar](50) NOT NULL,
	[ProjectName] [nvarchar](50) NOT NULL,
	[ProjectManagerID] [nvarchar](20) NOT NULL,
	[ProductManagerID] [nvarchar](20) NOT NULL,
	[ProjectType] [nvarchar](10) NULL,
	[ProjectLevel] [nvarchar](10) NULL,
	[ProjectKind] [nvarchar](10) NULL,
	[ProductionLineAttribute] [nvarchar](20) NULL,
	[ProjectStatus] [nvarchar](20) NULL,
	[IsEnabled] [bit] NOT NULL,
	[EffectiveDate] [datetime] NOT NULL,
	[ExpirationDate] [datetime] NOT NULL,
	[Creator] [nvarchar](20) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[Creator] [nvarchar](50) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](50) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RoleDepartment]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleDepartment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[DepartmentId] [nvarchar](50) NOT NULL,
	[Creator] [nvarchar](50) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](50) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_RoleDepartment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RoleModule]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleModule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Creator] [nvarchar](50) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](50) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_RoleModule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Team]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TeamName] [nvarchar](50) NOT NULL,
	[DepartmentID] [nvarchar](50) NOT NULL,
	[TeamLeader] [nvarchar](50) NOT NULL,
	[Creator] [nvarchar](20) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Timesheet]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Timesheet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TimesheetUser] [nvarchar](max) NULL,
	[TimesheetDate] [datetime] NULL,
	[ProjectID] [int] NULL,
	[ProjectGroup] [nvarchar](max) NULL,
	[Workload] [decimal](18, 2) NULL,
	[WorkContent] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[Approver] [nvarchar](max) NULL,
	[ApprovedTime] [datetime] NULL,
	[WorkflowInstanceID] [nvarchar](max) NULL,
	[Creator] [nvarchar](max) NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](max) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Timesheet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](50) NOT NULL,
	[RoleId] [int] NOT NULL,
	[Creator] [nvarchar](50) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](50) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserSetting]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSetting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](50) NOT NULL,
	[TeamId] [int] NOT NULL,
	[Creator] [nvarchar](20) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_UserSetting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[HRActiveDeptManagerV]    Script Date: 2019/11/24 12:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[HRActiveDeptManagerV]
AS 
SELECT  [FullDeptCode]
      ,[FullDeptName]
      ,[IsActiveDept]
      ,[DeptCode1]
      ,[DeptName1]
      ,[DeptLay1]
      ,[DeptCode2]
      ,[DeptName2]
      ,[DeptLay2]
      ,[DeptCode3]
      ,[DeptName3]
      ,[DeptLay3]
      ,[DeptCode4]
      ,[DeptName4]
      ,[DeptLay4]
      ,[UserID]
      ,[UserNum]
      ,[UserName]
      ,[FirstName]
      ,[LastName]
      ,[Email]
      ,[MobilePhone]
      ,[UserDesc]
      ,[UserDisplayName]
      ,[LongUserDisplayName]
      ,[CreationDate]
      ,[CreatedBy]
      ,[LastUpdateDate]
      ,[LastUpdatedBy]
      ,[LastUpdateLogin]
      ,[UserID2]
      ,[UserNum2]
  FROM [MAPSysDB].[dbo].[HRActiveDeptManagerV]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [HREmployee_N3]    Script Date: 2019/11/24 12:07:13 ******/
CREATE NONCLUSTERED INDEX [HREmployee_N3] ON [dbo].[HREmployee]
(
	[EmployeeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [HREmployee_N5]    Script Date: 2019/11/24 12:07:13 ******/
CREATE NONCLUSTERED INDEX [HREmployee_N5] ON [dbo].[HREmployee]
(
	[OrgCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [HREmployee_N6]    Script Date: 2019/11/24 12:07:13 ******/
CREATE NONCLUSTERED INDEX [HREmployee_N6] ON [dbo].[HREmployee]
(
	[DeptCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [HREmployee_U2]    Script Date: 2019/11/24 12:07:13 ******/
CREATE UNIQUE NONCLUSTERED INDEX [HREmployee_U2] ON [dbo].[HREmployee]
(
	[EmployeeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [HREmployee_U4]    Script Date: 2019/11/24 12:07:13 ******/
CREATE UNIQUE NONCLUSTERED INDEX [HREmployee_U4] ON [dbo].[HREmployee]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [ZNVTimesheet] SET  READ_WRITE 
GO

--获取所有在职的部门manager，只取工时表日期区间内没有记录的manager
--EXEC [dbo].[Proc_GetDepartmentManagerList] '2019-12-04','2019-12-08'
CREATE PROCEDURE [dbo].[Proc_GetDepartmentManagerList](
  @dateFrom AS datetime,  --开始日期
	@dateTo AS datetime		--结束日期
)
AS
BEGIN
	select DISTINCT a.UserNum from (select * from HRActiveDeptManagerV where FullDeptCode like '10000.11000%' and IsActiveDept = 'Y') a 
	left join (select * from Timesheet where TimesheetDate BETWEEN @dateFrom and @dateTo) b on a.UserNum=b.TimesheetUser 
	where b.TimesheetDate is NULL 
END