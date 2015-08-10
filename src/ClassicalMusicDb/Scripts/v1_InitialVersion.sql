USE [master]
GO
/****** Object:  Database [ClassicalMusicDb]    Script Date: 8/9/2015 9:32:46 PM ******/
CREATE DATABASE [ClassicalMusicDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Music', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\ClassicalMusicDb.mdf' , SIZE = 996544KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Music_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\ClassicalMusicDb_log.ldf' , SIZE = 4715200KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ClassicalMusicDb] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ClassicalMusicDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ClassicalMusicDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ClassicalMusicDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ClassicalMusicDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ClassicalMusicDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ClassicalMusicDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ClassicalMusicDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET RECOVERY FULL 
GO
ALTER DATABASE [ClassicalMusicDb] SET  MULTI_USER 
GO
ALTER DATABASE [ClassicalMusicDb] SET PAGE_VERIFY NONE  
GO
ALTER DATABASE [ClassicalMusicDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ClassicalMusicDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ClassicalMusicDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ClassicalMusicDb] SET DELAYED_DURABILITY = DISABLED 
GO
USE [ClassicalMusicDb]
GO
/****** Object:  User [papa]    Script Date: 8/9/2015 9:32:46 PM ******/
CREATE USER [papa] FOR LOGIN [papa] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Album]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Album](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Album] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Catalog]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Catalog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Prefix] [nvarchar](max) NOT NULL,
	[ComposerId] [int] NOT NULL,
 CONSTRAINT [PK_Catalog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CatalogNumber]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CatalogNumber](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[CatalogId] [int] NOT NULL,
 CONSTRAINT [PK_CatalogNumber] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Composer]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Composer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Dates] [nvarchar](max) NOT NULL,
	[BirthLocationId] [int] NULL,
	[DeathLocationId] [int] NULL,
	[Biography] [nvarchar](max) NULL,
	[IsPopular] [bit] NOT NULL,
 CONSTRAINT [PK_Composer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ComposerEra]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComposerEra](
	[EraId] [int] NOT NULL,
	[ComposerId] [int] NOT NULL,
 CONSTRAINT [PK_ComposerEra] PRIMARY KEY CLUSTERED 
(
	[EraId] ASC,
	[ComposerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ComposerImage]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ComposerImage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Bytes] [varbinary](max) NOT NULL,
	[ComposerId] [int] NOT NULL,
 CONSTRAINT [PK_ComposerImage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ComposerInfluence]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComposerInfluence](
	[ComposerId] [int] NOT NULL,
	[InfluenceId] [int] NOT NULL,
 CONSTRAINT [PK_ComposerInfluence] PRIMARY KEY CLUSTERED 
(
	[ComposerId] ASC,
	[InfluenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ComposerLink]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComposerLink](
	[LinkId] [int] NOT NULL,
	[ComposerId] [int] NOT NULL,
 CONSTRAINT [PK_ComposerLink] PRIMARY KEY CLUSTERED 
(
	[LinkId] ASC,
	[ComposerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ComposerNationality]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComposerNationality](
	[ComposerId] [int] NOT NULL,
	[NationalityId] [int] NOT NULL,
 CONSTRAINT [PK_ComposerNationality] PRIMARY KEY CLUSTERED 
(
	[ComposerId] ASC,
	[NationalityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Composition]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Composition](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Dates] [nvarchar](max) NULL,
	[Nickname] [nvarchar](max) NULL,
	[Premiere] [nvarchar](max) NULL,
	[Dedication] [nvarchar](max) NULL,
	[Occasion] [nvarchar](max) NULL,
	[IsPopular] [bit] NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[KeyId] [int] NULL,
	[GenreId] [int] NULL,
	[InstrumentationId] [int] NULL,
	[CompositionCollectionId] [int] NULL,
 CONSTRAINT [PK_Composition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompositionCatalogNumber]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompositionCatalogNumber](
	[CompositionId] [int] NOT NULL,
	[CatalogNumberId] [int] NOT NULL,
 CONSTRAINT [PK_CompostionCatalogNumber] PRIMARY KEY CLUSTERED 
(
	[CompositionId] ASC,
	[CatalogNumberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompositionCollection]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompositionCollection](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[IsPopular] [bit] NOT NULL,
 CONSTRAINT [PK_CompositionCollection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompositionCollectionCatalogNumber]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompositionCollectionCatalogNumber](
	[CompositionCollectionId] [int] NOT NULL,
	[CatalogNumberId] [int] NOT NULL,
 CONSTRAINT [PK_CompositionCollectionCatalogNumber] PRIMARY KEY CLUSTERED 
(
	[CompositionCollectionId] ASC,
	[CatalogNumberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompositionCollectionComposer]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompositionCollectionComposer](
	[ComposerId] [int] NOT NULL,
	[CompositionCollectionId] [int] NOT NULL,
 CONSTRAINT [PK_CompositionCollectionComposer] PRIMARY KEY CLUSTERED 
(
	[ComposerId] ASC,
	[CompositionCollectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompositionCollectionRecording]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompositionCollectionRecording](
	[CompositionCollectionId] [int] NOT NULL,
	[RecordingId] [int] NOT NULL,
 CONSTRAINT [PK_CompositionCollectionRecording] PRIMARY KEY CLUSTERED 
(
	[CompositionCollectionId] ASC,
	[RecordingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompositionComposer]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompositionComposer](
	[ComposerId] [int] NOT NULL,
	[CompositionId] [int] NOT NULL,
 CONSTRAINT [PK_CompositionComposer] PRIMARY KEY CLUSTERED 
(
	[ComposerId] ASC,
	[CompositionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompositionLink]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompositionLink](
	[LinkId] [int] NOT NULL,
	[CompositionId] [int] NOT NULL,
 CONSTRAINT [PK_CompositionLink] PRIMARY KEY CLUSTERED 
(
	[LinkId] ASC,
	[CompositionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompositionRecording]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompositionRecording](
	[CompositionId] [int] NOT NULL,
	[RecordingId] [int] NOT NULL,
 CONSTRAINT [PK_CompositionRecording] PRIMARY KEY CLUSTERED 
(
	[CompositionId] ASC,
	[RecordingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Era]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Era](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Dates] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Era] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Genre]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genre](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Genre] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Instrumentation]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instrumentation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Instrumentation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Key]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Key](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Key] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Link]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Link](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Link] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Location]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Movement]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[IsPopular] [bit] NOT NULL,
	[CompositionId] [int] NOT NULL,
 CONSTRAINT [PK_Movement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MovementRecording]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovementRecording](
	[MovementId] [int] NOT NULL,
	[RecordingId] [int] NOT NULL,
 CONSTRAINT [PK_MovementRecording] PRIMARY KEY CLUSTERED 
(
	[MovementId] ASC,
	[RecordingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Nationality]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nationality](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Nationality] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Performer]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Performer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Performer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Recording]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recording](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrackNumber] [int] NULL,
	[Dates] [nvarchar](max) NOT NULL,
	[AlbumId] [int] NULL,
 CONSTRAINT [PK_Recording] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RecordingLocation]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordingLocation](
	[RecordingId] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
 CONSTRAINT [PK_RecordingLocation] PRIMARY KEY CLUSTERED 
(
	[RecordingId] ASC,
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RecordingPerformer]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordingPerformer](
	[RecordingId] [int] NOT NULL,
	[PerformerId] [int] NOT NULL,
 CONSTRAINT [PK_RecordingPerformer] PRIMARY KEY CLUSTERED 
(
	[RecordingId] ASC,
	[PerformerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sample]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sample](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Artists] [nvarchar](max) NOT NULL,
	[Bytes] [varbinary](max) NOT NULL,
	[ComposerID] [int] NOT NULL,
 CONSTRAINT [PK_Sample] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Catalog]  WITH CHECK ADD  CONSTRAINT [FK_Catalog_Composer] FOREIGN KEY([ComposerId])
REFERENCES [dbo].[Composer] ([Id])
GO
ALTER TABLE [dbo].[Catalog] CHECK CONSTRAINT [FK_Catalog_Composer]
GO
ALTER TABLE [dbo].[CatalogNumber]  WITH CHECK ADD  CONSTRAINT [FK_CatalogNumber_Catalog] FOREIGN KEY([CatalogId])
REFERENCES [dbo].[Catalog] ([Id])
GO
ALTER TABLE [dbo].[CatalogNumber] CHECK CONSTRAINT [FK_CatalogNumber_Catalog]
GO
ALTER TABLE [dbo].[Composer]  WITH CHECK ADD  CONSTRAINT [FK_Composer_Location] FOREIGN KEY([BirthLocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Composer] CHECK CONSTRAINT [FK_Composer_Location]
GO
ALTER TABLE [dbo].[Composer]  WITH CHECK ADD  CONSTRAINT [FK_Composer_Location1] FOREIGN KEY([DeathLocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Composer] CHECK CONSTRAINT [FK_Composer_Location1]
GO
ALTER TABLE [dbo].[ComposerEra]  WITH CHECK ADD  CONSTRAINT [FK_ComposerEra_Composer] FOREIGN KEY([ComposerId])
REFERENCES [dbo].[Composer] ([Id])
GO
ALTER TABLE [dbo].[ComposerEra] CHECK CONSTRAINT [FK_ComposerEra_Composer]
GO
ALTER TABLE [dbo].[ComposerEra]  WITH CHECK ADD  CONSTRAINT [FK_ComposerEra_Era] FOREIGN KEY([EraId])
REFERENCES [dbo].[Era] ([Id])
GO
ALTER TABLE [dbo].[ComposerEra] CHECK CONSTRAINT [FK_ComposerEra_Era]
GO
ALTER TABLE [dbo].[ComposerImage]  WITH CHECK ADD  CONSTRAINT [FK_ComposerImage_Composer] FOREIGN KEY([ComposerId])
REFERENCES [dbo].[Composer] ([Id])
GO
ALTER TABLE [dbo].[ComposerImage] CHECK CONSTRAINT [FK_ComposerImage_Composer]
GO
ALTER TABLE [dbo].[ComposerInfluence]  WITH CHECK ADD  CONSTRAINT [FK_ComposerInfluence_Composer] FOREIGN KEY([ComposerId])
REFERENCES [dbo].[Composer] ([Id])
GO
ALTER TABLE [dbo].[ComposerInfluence] CHECK CONSTRAINT [FK_ComposerInfluence_Composer]
GO
ALTER TABLE [dbo].[ComposerInfluence]  WITH CHECK ADD  CONSTRAINT [FK_ComposerInfluence_Composer1] FOREIGN KEY([InfluenceId])
REFERENCES [dbo].[Composer] ([Id])
GO
ALTER TABLE [dbo].[ComposerInfluence] CHECK CONSTRAINT [FK_ComposerInfluence_Composer1]
GO
ALTER TABLE [dbo].[ComposerLink]  WITH CHECK ADD  CONSTRAINT [FK_ComposerLink_Composer] FOREIGN KEY([ComposerId])
REFERENCES [dbo].[Composer] ([Id])
GO
ALTER TABLE [dbo].[ComposerLink] CHECK CONSTRAINT [FK_ComposerLink_Composer]
GO
ALTER TABLE [dbo].[ComposerLink]  WITH CHECK ADD  CONSTRAINT [FK_ComposerLink_Link] FOREIGN KEY([LinkId])
REFERENCES [dbo].[Link] ([Id])
GO
ALTER TABLE [dbo].[ComposerLink] CHECK CONSTRAINT [FK_ComposerLink_Link]
GO
ALTER TABLE [dbo].[ComposerNationality]  WITH CHECK ADD  CONSTRAINT [FK_ComposerNationality_Composer] FOREIGN KEY([ComposerId])
REFERENCES [dbo].[Composer] ([Id])
GO
ALTER TABLE [dbo].[ComposerNationality] CHECK CONSTRAINT [FK_ComposerNationality_Composer]
GO
ALTER TABLE [dbo].[ComposerNationality]  WITH CHECK ADD  CONSTRAINT [FK_ComposerNationality_Nationality] FOREIGN KEY([NationalityId])
REFERENCES [dbo].[Nationality] ([Id])
GO
ALTER TABLE [dbo].[ComposerNationality] CHECK CONSTRAINT [FK_ComposerNationality_Nationality]
GO
ALTER TABLE [dbo].[Composition]  WITH CHECK ADD  CONSTRAINT [FK_Composition_CompositionCollection] FOREIGN KEY([CompositionCollectionId])
REFERENCES [dbo].[CompositionCollection] ([Id])
GO
ALTER TABLE [dbo].[Composition] CHECK CONSTRAINT [FK_Composition_CompositionCollection]
GO
ALTER TABLE [dbo].[Composition]  WITH CHECK ADD  CONSTRAINT [FK_Composition_Genre] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genre] ([Id])
GO
ALTER TABLE [dbo].[Composition] CHECK CONSTRAINT [FK_Composition_Genre]
GO
ALTER TABLE [dbo].[Composition]  WITH CHECK ADD  CONSTRAINT [FK_Composition_Instrumentation] FOREIGN KEY([InstrumentationId])
REFERENCES [dbo].[Instrumentation] ([Id])
GO
ALTER TABLE [dbo].[Composition] CHECK CONSTRAINT [FK_Composition_Instrumentation]
GO
ALTER TABLE [dbo].[Composition]  WITH CHECK ADD  CONSTRAINT [FK_Composition_Key] FOREIGN KEY([KeyId])
REFERENCES [dbo].[Key] ([Id])
GO
ALTER TABLE [dbo].[Composition] CHECK CONSTRAINT [FK_Composition_Key]
GO
ALTER TABLE [dbo].[CompositionCatalogNumber]  WITH CHECK ADD  CONSTRAINT [FK_CompostionCatalogNumber_CatalogNumber] FOREIGN KEY([CatalogNumberId])
REFERENCES [dbo].[CatalogNumber] ([Id])
GO
ALTER TABLE [dbo].[CompositionCatalogNumber] CHECK CONSTRAINT [FK_CompostionCatalogNumber_CatalogNumber]
GO
ALTER TABLE [dbo].[CompositionCatalogNumber]  WITH CHECK ADD  CONSTRAINT [FK_CompostionCatalogNumber_Composition] FOREIGN KEY([CompositionId])
REFERENCES [dbo].[Composition] ([Id])
GO
ALTER TABLE [dbo].[CompositionCatalogNumber] CHECK CONSTRAINT [FK_CompostionCatalogNumber_Composition]
GO
ALTER TABLE [dbo].[CompositionCollectionCatalogNumber]  WITH CHECK ADD  CONSTRAINT [FK_CompositionCollectionCatalogNumber_CatalogNumber] FOREIGN KEY([CatalogNumberId])
REFERENCES [dbo].[CatalogNumber] ([Id])
GO
ALTER TABLE [dbo].[CompositionCollectionCatalogNumber] CHECK CONSTRAINT [FK_CompositionCollectionCatalogNumber_CatalogNumber]
GO
ALTER TABLE [dbo].[CompositionCollectionCatalogNumber]  WITH CHECK ADD  CONSTRAINT [FK_CompositionCollectionCatalogNumber_CompositionCollection] FOREIGN KEY([CompositionCollectionId])
REFERENCES [dbo].[CompositionCollection] ([Id])
GO
ALTER TABLE [dbo].[CompositionCollectionCatalogNumber] CHECK CONSTRAINT [FK_CompositionCollectionCatalogNumber_CompositionCollection]
GO
ALTER TABLE [dbo].[CompositionCollectionComposer]  WITH CHECK ADD  CONSTRAINT [FK_CompositionCollectionComposer_Composer] FOREIGN KEY([ComposerId])
REFERENCES [dbo].[Composer] ([Id])
GO
ALTER TABLE [dbo].[CompositionCollectionComposer] CHECK CONSTRAINT [FK_CompositionCollectionComposer_Composer]
GO
ALTER TABLE [dbo].[CompositionCollectionComposer]  WITH CHECK ADD  CONSTRAINT [FK_CompositionCollectionComposer_CompositionCollection] FOREIGN KEY([CompositionCollectionId])
REFERENCES [dbo].[CompositionCollection] ([Id])
GO
ALTER TABLE [dbo].[CompositionCollectionComposer] CHECK CONSTRAINT [FK_CompositionCollectionComposer_CompositionCollection]
GO
ALTER TABLE [dbo].[CompositionCollectionRecording]  WITH CHECK ADD  CONSTRAINT [FK_CompositionCollectionRecording_CompositionCollectionRecording] FOREIGN KEY([CompositionCollectionId])
REFERENCES [dbo].[CompositionCollection] ([Id])
GO
ALTER TABLE [dbo].[CompositionCollectionRecording] CHECK CONSTRAINT [FK_CompositionCollectionRecording_CompositionCollectionRecording]
GO
ALTER TABLE [dbo].[CompositionCollectionRecording]  WITH CHECK ADD  CONSTRAINT [FK_CompositionCollectionRecording_Recording] FOREIGN KEY([RecordingId])
REFERENCES [dbo].[Recording] ([Id])
GO
ALTER TABLE [dbo].[CompositionCollectionRecording] CHECK CONSTRAINT [FK_CompositionCollectionRecording_Recording]
GO
ALTER TABLE [dbo].[CompositionComposer]  WITH CHECK ADD  CONSTRAINT [FK_CompositionComposer_Composer] FOREIGN KEY([ComposerId])
REFERENCES [dbo].[Composer] ([Id])
GO
ALTER TABLE [dbo].[CompositionComposer] CHECK CONSTRAINT [FK_CompositionComposer_Composer]
GO
ALTER TABLE [dbo].[CompositionComposer]  WITH CHECK ADD  CONSTRAINT [FK_CompositionComposer_Composition] FOREIGN KEY([CompositionId])
REFERENCES [dbo].[Composition] ([Id])
GO
ALTER TABLE [dbo].[CompositionComposer] CHECK CONSTRAINT [FK_CompositionComposer_Composition]
GO
ALTER TABLE [dbo].[CompositionLink]  WITH CHECK ADD  CONSTRAINT [FK_CompositionLink_Composition] FOREIGN KEY([CompositionId])
REFERENCES [dbo].[Composition] ([Id])
GO
ALTER TABLE [dbo].[CompositionLink] CHECK CONSTRAINT [FK_CompositionLink_Composition]
GO
ALTER TABLE [dbo].[CompositionLink]  WITH CHECK ADD  CONSTRAINT [FK_CompositionLink_Link] FOREIGN KEY([LinkId])
REFERENCES [dbo].[Link] ([Id])
GO
ALTER TABLE [dbo].[CompositionLink] CHECK CONSTRAINT [FK_CompositionLink_Link]
GO
ALTER TABLE [dbo].[CompositionRecording]  WITH CHECK ADD  CONSTRAINT [FK_CompositionRecording_Composition] FOREIGN KEY([CompositionId])
REFERENCES [dbo].[Composition] ([Id])
GO
ALTER TABLE [dbo].[CompositionRecording] CHECK CONSTRAINT [FK_CompositionRecording_Composition]
GO
ALTER TABLE [dbo].[CompositionRecording]  WITH CHECK ADD  CONSTRAINT [FK_CompositionRecording_Recording] FOREIGN KEY([RecordingId])
REFERENCES [dbo].[Recording] ([Id])
GO
ALTER TABLE [dbo].[CompositionRecording] CHECK CONSTRAINT [FK_CompositionRecording_Recording]
GO
ALTER TABLE [dbo].[Movement]  WITH CHECK ADD  CONSTRAINT [FK_Movement_Composition] FOREIGN KEY([CompositionId])
REFERENCES [dbo].[Composition] ([Id])
GO
ALTER TABLE [dbo].[Movement] CHECK CONSTRAINT [FK_Movement_Composition]
GO
ALTER TABLE [dbo].[MovementRecording]  WITH CHECK ADD  CONSTRAINT [FK_MovementRecording_Movement] FOREIGN KEY([MovementId])
REFERENCES [dbo].[Movement] ([Id])
GO
ALTER TABLE [dbo].[MovementRecording] CHECK CONSTRAINT [FK_MovementRecording_Movement]
GO
ALTER TABLE [dbo].[MovementRecording]  WITH CHECK ADD  CONSTRAINT [FK_MovementRecording_Recording] FOREIGN KEY([RecordingId])
REFERENCES [dbo].[Recording] ([Id])
GO
ALTER TABLE [dbo].[MovementRecording] CHECK CONSTRAINT [FK_MovementRecording_Recording]
GO
ALTER TABLE [dbo].[Recording]  WITH CHECK ADD  CONSTRAINT [FK_Recording_Album] FOREIGN KEY([AlbumId])
REFERENCES [dbo].[Album] ([Id])
GO
ALTER TABLE [dbo].[Recording] CHECK CONSTRAINT [FK_Recording_Album]
GO
ALTER TABLE [dbo].[RecordingLocation]  WITH CHECK ADD  CONSTRAINT [FK_RecordingLocation_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[RecordingLocation] CHECK CONSTRAINT [FK_RecordingLocation_Location]
GO
ALTER TABLE [dbo].[RecordingLocation]  WITH CHECK ADD  CONSTRAINT [FK_RecordingLocation_Recording] FOREIGN KEY([RecordingId])
REFERENCES [dbo].[Recording] ([Id])
GO
ALTER TABLE [dbo].[RecordingLocation] CHECK CONSTRAINT [FK_RecordingLocation_Recording]
GO
ALTER TABLE [dbo].[RecordingPerformer]  WITH CHECK ADD  CONSTRAINT [FK_RecordingPerformer_Performer] FOREIGN KEY([PerformerId])
REFERENCES [dbo].[Performer] ([Id])
GO
ALTER TABLE [dbo].[RecordingPerformer] CHECK CONSTRAINT [FK_RecordingPerformer_Performer]
GO
ALTER TABLE [dbo].[RecordingPerformer]  WITH CHECK ADD  CONSTRAINT [FK_RecordingPerformer_Recording] FOREIGN KEY([RecordingId])
REFERENCES [dbo].[Recording] ([Id])
GO
ALTER TABLE [dbo].[RecordingPerformer] CHECK CONSTRAINT [FK_RecordingPerformer_Recording]
GO
ALTER TABLE [dbo].[Sample]  WITH CHECK ADD  CONSTRAINT [FK_Sample_Composer] FOREIGN KEY([ComposerID])
REFERENCES [dbo].[Composer] ([Id])
GO
ALTER TABLE [dbo].[Sample] CHECK CONSTRAINT [FK_Sample_Composer]
GO
/****** Object:  StoredProcedure [dbo].[GenerateHistoryTable]    Script Date: 8/9/2015 9:32:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GenerateHistoryTable]
	@TableName varchar(128),
	@Owner varchar(128) = 'dbo',
	@AuditNameExtention varchar(128) = 'History',
	@DropAuditTable bit = 0
AS
BEGIN

	-- Check if table exists
	IF not exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[' + @TableName + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		PRINT 'ERROR: Table does not exist'
		RETURN
	END

	-- Check @AuditNameExtention
	IF @AuditNameExtention is null
	BEGIN
		PRINT 'ERROR: @AuditNameExtention cannot be null'
		RETURN
	END

	-- Drop audit table if it exists and drop should be forced
	IF (exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[' + @TableName + @AuditNameExtention + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1) and @DropAuditTable = 1)
	BEGIN
		PRINT 'Dropping audit table [' + @Owner + '].[' + @TableName + @AuditNameExtention + ']'
		EXEC ('drop table ' + @TableName + @AuditNameExtention)
	END

	-- Declare cursor to loop over columns
	DECLARE TableColumns CURSOR Read_Only
	FOR SELECT b.name, c.name as TypeName, b.length, b.isnullable, b.collation, b.xprec, b.xscale
		FROM sysobjects a 
		inner join syscolumns b on a.id = b.id 
		inner join systypes c on b.xtype = c.xtype and c.name <> 'sysname' 
		WHERE a.id = object_id(N'[' + @Owner + '].[' + @TableName + ']') 
		and OBJECTPROPERTY(a.id, N'IsUserTable') = 1 
		ORDER BY b.colId

	OPEN TableColumns

	-- Declare temp variable to fetch records into
	DECLARE @ColumnName varchar(128)
	DECLARE @ColumnType varchar(128)
	DECLARE @ColumnLength smallint
	DECLARE @ColumnNullable int
	DECLARE @ColumnCollation sysname
	DECLARE @ColumnPrecision tinyint
	DECLARE @ColumnScale tinyint

	-- Declare variable to build statements
	DECLARE @CreateStatement varchar(8000)
	DECLARE @ListOfFields varchar(2000)
	SET @ListOfFields = ''


	-- Check if audit table exists
	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[' + @TableName + @AuditNameExtention + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		-- AuditTable exists, update needed
		PRINT 'Table already exists. Only triggers will be updated.'

		FETCH Next FROM TableColumns
		INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF (@ColumnType <> 'text' and @ColumnType <> 'ntext' and @ColumnType <> 'image' and @ColumnType <> 'timestamp')
			BEGIN
				SET @ListOfFields = @ListOfFields + @ColumnName + ','
			END

			FETCH Next FROM TableColumns
			INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale

		END
	END
	ELSE
	BEGIN
		-- AuditTable does not exist, create new

		-- Start of create table
		SET @CreateStatement = 'CREATE TABLE [' + @Owner + '].[' + @TableName + @AuditNameExtention + '] ('
		SET @CreateStatement = @CreateStatement + '[AuditID] [bigint] IDENTITY (1, 1) NOT NULL,'

		FETCH Next FROM TableColumns
		INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF (@ColumnType <> 'text' and @ColumnType <> 'ntext' and @ColumnType <> 'image' and @ColumnType <> 'timestamp')
			BEGIN
				SET @ListOfFields = @ListOfFields + @ColumnName + ','
		
				SET @CreateStatement = @CreateStatement + '[' + @ColumnName + '] [' + @ColumnType + '] '
				
				IF @ColumnType in ('binary', 'char', 'nchar', 'nvarchar', 'varbinary', 'varchar')
				BEGIN
					IF (@ColumnLength = -1)
						Set @CreateStatement = @CreateStatement + '(max) '	 	
					ELSE
						SET @CreateStatement = @CreateStatement + '(' + cast(@ColumnLength as varchar(10)) + ') '	 	
				END
		
				IF @ColumnType in ('decimal', 'numeric')
					SET @CreateStatement = @CreateStatement + '(' + cast(@ColumnPrecision as varchar(10)) + ',' + cast(@ColumnScale as varchar(10)) + ') '	 	
		
				IF @ColumnType in ('char', 'nchar', 'nvarchar', 'varchar', 'text', 'ntext')
					SET @CreateStatement = @CreateStatement + 'COLLATE ' + @ColumnCollation + ' '
		
				IF @ColumnNullable = 0
					SET @CreateStatement = @CreateStatement + 'NOT '	 	
		
				SET @CreateStatement = @CreateStatement + 'NULL, '	 	
			END

			FETCH Next FROM TableColumns
			INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale
		END
		
		-- Add audit trail columns
		SET @CreateStatement = @CreateStatement + '[Operation] [char] (6) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,'
		SET @CreateStatement = @CreateStatement + '[Time] [datetime] NOT NULL ,'
		SET @CreateStatement = @CreateStatement + '[User] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,'
		SET @CreateStatement = @CreateStatement + '[Location] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL)' 

		-- Create audit table
		PRINT 'Creating audit table [' + @Owner + '].[' + @TableName + @AuditNameExtention + ']'
		EXEC (@CreateStatement)

		-- Set primary key and default values
		SET @CreateStatement = 'ALTER TABLE [' + @Owner + '].[' + @TableName + @AuditNameExtention + '] ADD '
		SET @CreateStatement = @CreateStatement + 'CONSTRAINT [DF_' + @TableName + @AuditNameExtention + '_Time] DEFAULT (SYSDATETIMEOFFSET()) FOR [Time],'
		SET @CreateStatement = @CreateStatement + 'CONSTRAINT [DF_' + @TableName + @AuditNameExtention + '_User] DEFAULT (suser_sname()) FOR [User],CONSTRAINT [PK_' + @TableName + @AuditNameExtention + '] PRIMARY KEY  CLUSTERED '
		SET @CreateStatement = @CreateStatement + '([AuditID])  ON [PRIMARY], '
		SET @CreateStatement = @CreateStatement + 'CONSTRAINT [DF_' + @TableName + @AuditNameExtention + '_Location]  DEFAULT (CONVERT(NVARCHAR, CONNECTIONPROPERTY(''client_net_address''))) for [Location]'

		EXEC (@CreateStatement)

	END

	CLOSE TableColumns
	DEALLOCATE TableColumns

	/* Drop Triggers, if they exist */
	PRINT 'Dropping triggers'
	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TR_' + @TableName + '_Insert]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TR_' + @TableName + '_Insert]')

	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TR_' + @TableName + '_Update]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TR_' + @TableName + '_Update]')

	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TR_' + @TableName + '_Delete]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TR_' + @TableName + '_Delete]')

	/* Create triggers */
	PRINT 'Creating triggers' 
	EXEC ('CREATE TRIGGER TR_' + @TableName + '_Insert ON ' + '[' + @Owner + '].[' + @TableName + '] FOR INSERT AS INSERT INTO ' + @TableName + @AuditNameExtention + '(' +  @ListOfFields + 'Operation) SELECT ' + @ListOfFields + '''Insert'' FROM Inserted')

	EXEC ('CREATE TRIGGER TR_' + @TableName + '_Update ON ' +  '[' + @Owner + '].[' + @TableName + '] FOR UPDATE AS INSERT INTO ' + @TableName + @AuditNameExtention + '(' +  @ListOfFields + 'Operation) SELECT ' + @ListOfFields + '''Update'' FROM Inserted')

	EXEC ('CREATE TRIGGER TR_' + @TableName + '_Delete ON ' +  '[' + @Owner + '].[' + @TableName + '] FOR DELETE AS INSERT INTO ' + @TableName + @AuditNameExtention + '(' +  @ListOfFields + 'Operation) SELECT ' + @ListOfFields + '''Delete'' FROM Deleted')

END

GO
USE [master]
GO
ALTER DATABASE [ClassicalMusicDb] SET  READ_WRITE 
GO
