USE [ZNVTimesheet]
GO
/****** Object:  Table [dbo].[EmailTemplate]    Script Date: 2019/7/28 12:18:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmailTemplateCode] [nvarchar](50) NOT NULL,
	[EmailTemplateName] [nvarchar](50) NOT NULL,
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
/****** Object:  Table [dbo].[Holiday]    Script Date: 2019/7/28 12:18:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Holiday](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HolidayDate] [date] NOT NULL,
	[HolidayType] [nvarchar](50) NOT NULL,
	[Creator] [nvarchar](20) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Holiday] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PermissionModule]    Script Date: 2019/7/28 12:18:22 ******/
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
/****** Object:  Table [dbo].[Project]    Script Date: 2019/7/28 12:18:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
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
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 2019/7/28 12:18:22 ******/
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
/****** Object:  Table [dbo].[RoleModule]    Script Date: 2019/7/28 12:18:22 ******/
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
/****** Object:  Table [dbo].[Team]    Script Date: 2019/7/28 12:18:22 ******/
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
/****** Object:  Table [dbo].[Timesheet]    Script Date: 2019/7/28 12:18:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Timesheet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TimesheetUser] [nvarchar](20) NOT NULL,
	[TimesheetDate] [date] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ProjectGroup] [nvarchar](20) NULL,
	[Workload] [decimal](18, 2) NOT NULL,
	[WorkContent] [nvarchar](200) NULL,
	[Remarks] [nvarchar](200) NULL,
	[Status] [nvarchar](50) NULL,
	[Approver] [nvarchar](50) NULL,
	[ApprovedTime] [datetime] NULL,
	[WorkflowInstanceID] [nvarchar](50) NULL,
	[Creator] [nvarchar](20) NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Timesheet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 2019/7/28 12:18:22 ******/
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
ALTER TABLE [dbo].[PermissionModule] ADD  CONSTRAINT [DF_PermissionModule_Level]  DEFAULT ((0)) FOR [Level]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节假日日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Holiday', @level2type=N'COLUMN',@level2name=N'HolidayDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作日变节假日 |  周末变工作日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Holiday', @level2type=N'COLUMN',@level2name=N'HolidayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否立项' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'IsApproval'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目经理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectManagerID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品经理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProductManagerID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目级别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目性质' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectKind'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产线属性' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProductionLineAttribute'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ProjectStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否投入' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'IsEnabled'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生效日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'EffectiveDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失效日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'ExpirationDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'Creator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'CreationTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后编辑人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'LastModifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'LastModifyTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'IsDeleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'科室名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Team', @level2type=N'COLUMN',@level2name=N'TeamName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Team', @level2type=N'COLUMN',@level2name=N'DepartmentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'科室领导' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Team', @level2type=N'COLUMN',@level2name=N'TeamLeader'
GO


USE [ZNVTimesheet]
GO
/****** Object:  StoredProcedure [dbo].[Proc_DepartmentReport]    Script Date: 2019/8/5 7:17:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[Proc_DepartmentReport]
(
	@startDate datetime,
	@endDate datetime
	--@departmentIDs nvarchar(max) -- 多个departmentID 以,号分开， 如果为空则查询所有
)
as
begin
	--with cte as
	--(
	--	SELECT    B.departmentID
	--	  FROM      ( SELECT    [value] = CONVERT(XML , '<v>' + REPLACE(@departmentIDs , ',' , '</v><v>')
	--							+ '</v>')
	--				) A
	--	  OUTER APPLY ( SELECT  departmentID = N.v.value('.' , 'varchar(100)')
	--					FROM    A.[value].nodes('/v') N ( v )
	--				  ) B
 --   )

	DECLARE @ColumnGroup NVARCHAR(MAX), @PivotSQL NVARCHAR(MAX) 


	SELECT D.DeptCode1 as '部门编码'
	  , D.DeptName1 as '部门名称'
	  ,[TimesheetUser] as '人员姓名'
	  , C.EmployeeName as '人员编号'
	  ,SUM([Workload]) AS Workload
	  , B.ProjectName
	INTO #TempTimesheet
	FROM [Timesheet] A
		INNER JOIN [Project] B ON A.ProjectID = B.Id
		INNER JOIN [MAPSysDB].[dbo].[HREmployee] C ON C.EmployeeCode = A.TimesheetUser
		INNER JOIN [MAPSysDB].[dbo].[HRDeptTree] D ON D.DeptCode1 = C.DeptCode
	--WHERE A.TimesheetDate >= @startDate and A.TimesheetDate <= @endDate
	GROUP BY [TimesheetUser]
      ,[ProjectID]
	  , B.ProjectName
	  , D.DeptCode1
	  , D.DeptName1
	  , C.EmployeeName
	ORDER BY D.DeptName1, C.EmployeeName

	SELECT @ColumnGroup = COALESCE(@ColumnGroup + ',' ,'' ) + QUOTENAME(ProjectName) 
	FROM #TempTimesheet
	GROUP BY QUOTENAME(ProjectName) 

	SELECT @PivotSQL = N'
	SELECT * FROM #TempTimesheet PIVOT (SUM(Workload) FOR ProjectName 
	 IN (' + @ColumnGroup +  N') ) AS pvt' 

	EXEC sp_executesql  @PivotSQL;

	drop table #TempTimesheet
end
GO
