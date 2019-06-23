USE [master]
GO
/****** Object:  Database [ZNVTimesheet]    Script Date: 2019/6/24 7:05:55 ******/
CREATE DATABASE [ZNVTimesheet]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ZNVTimesheet', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL11.DEV\MSSQL\DATA\ZNVTimesheet.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ZNVTimesheet_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL11.DEV\MSSQL\DATA\ZNVTimesheet_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ZNVTimesheet] SET COMPATIBILITY_LEVEL = 110
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
ALTER DATABASE [ZNVTimesheet] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ZNVTimesheet] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ZNVTimesheet', N'ON'
GO
USE [ZNVTimesheet]
GO
/****** Object:  Table [dbo].[Holiday]    Script Date: 2019/6/24 7:05:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Holiday](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HolidayDate] [date] NOT NULL,
	[HolidayType] [nvarchar](50) NOT NULL,
	[Creator] [nvarchar](20) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Holiday] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Project]    Script Date: 2019/6/24 7:05:55 ******/
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
/****** Object:  Table [dbo].[Team]    Script Date: 2019/6/24 7:05:55 ******/
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
	[CreatedTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Timesheet]    Script Date: 2019/6/24 7:05:55 ******/
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
	[CreatedTime] [datetime] NOT NULL,
	[LastModifier] [nvarchar](20) NULL,
	[LastModifyTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Timesheet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

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
USE [master]
GO
ALTER DATABASE [ZNVTimesheet] SET  READ_WRITE 
GO
