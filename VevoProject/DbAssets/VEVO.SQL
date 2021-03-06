USE [master]
GO
/****** Object:  Database [VevoNew]    Script Date: 5/14/2015 1:52:14 PM ******/
CREATE DATABASE [VevoNew]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'VevoNew', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\VevoNew.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'VevoNew_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\VevoNew_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VevoNew].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [VevoNew] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [VevoNew] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [VevoNew] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [VevoNew] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [VevoNew] SET ARITHABORT OFF 
GO
ALTER DATABASE [VevoNew] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [VevoNew] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [VevoNew] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [VevoNew] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [VevoNew] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [VevoNew] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [VevoNew] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [VevoNew] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [VevoNew] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [VevoNew] SET  DISABLE_BROKER 
GO
ALTER DATABASE [VevoNew] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [VevoNew] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [VevoNew] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [VevoNew] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [VevoNew] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [VevoNew] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [VevoNew] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [VevoNew] SET RECOVERY FULL 
GO
ALTER DATABASE [VevoNew] SET  MULTI_USER 
GO
ALTER DATABASE [VevoNew] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [VevoNew] SET DB_CHAINING OFF 
GO
ALTER DATABASE [VevoNew] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [VevoNew] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'VevoNew', N'ON'
GO
USE [VevoNew]
GO
/****** Object:  Table [dbo].[Artist]    Script Date: 5/14/2015 1:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artist](
	[ArtistId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UrlSafeName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Artist] PRIMARY KEY CLUSTERED 
(
	[ArtistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SampleData]    Script Date: 5/14/2015 1:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SampleData](
	[ISRC] [varchar](500) NULL,
	[Title] [varchar](500) NULL,
	[UrlSafeTitle] [varchar](500) NULL,
	[Artist1Name] [varchar](500) NULL,
	[Artist1UrlSafeName] [varchar](500) NULL,
	[ArtistId] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Video]    Script Date: 5/14/2015 1:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Video](
	[ISRC] [nvarchar](20) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[UrlSafeTitle] [nvarchar](200) NOT NULL,
	[ArtistId] [int] NOT NULL,
 CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED 
(
	[ISRC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_URL_NAME]    Script Date: 5/14/2015 1:52:14 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IDX_URL_NAME] ON [dbo].[Artist]
(
	[UrlSafeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_VID_IRTIST]    Script Date: 5/14/2015 1:52:14 PM ******/
CREATE NONCLUSTERED INDEX [IDX_VID_IRTIST] ON [dbo].[Video]
(
	[ArtistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Video]  WITH CHECK ADD  CONSTRAINT [FK_Video_Artist] FOREIGN KEY([ArtistId])
REFERENCES [dbo].[Artist] ([ArtistId])
GO
ALTER TABLE [dbo].[Video] CHECK CONSTRAINT [FK_Video_Artist]
GO
/****** Object:  StoredProcedure [dbo].[AddArtist]    Script Date: 5/14/2015 1:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[AddArtist](@artistName nvarchar(50), @urlSafeName nvarchar(100))
as
	
INSERT INTO [dbo].[Artist]
           ([Name]
           ,[UrlSafeName])
     VALUES
           (@artistName
           ,@urlSafeName)

select * from Artist where [UrlSafeName] = @urlSafeName;



GO
/****** Object:  StoredProcedure [dbo].[DeleteArtistByUrlSafeName]    Script Date: 5/14/2015 1:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[DeleteArtistByUrlSafeName](@urlSafeName nvarchar(max))
as
	Delete from Video 
	where ArtistId in (select artistId from Artist where
	 Artist.UrlSafeName = @urlSafeName);

	 delete from Artist where Artist.UrlSafeName = @urlSafeName;

 



GO
/****** Object:  StoredProcedure [dbo].[GetArtistByUrlSafeName]    Script Date: 5/14/2015 1:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[GetArtistByUrlSafeName](@urlSafeName nvarchar(max))
as
	select * from artist where UrlSafeName = @urlSafeName;
 


GO
/****** Object:  StoredProcedure [dbo].[GetVideo]    Script Date: 5/14/2015 1:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[GetVideo](@isrc nvarchar(max))
as
	select  
		video.ISRC, video.Title, video.UrlSafeTitle 
		
	from Video 	 where ISRC = @isrc
 




GO
/****** Object:  StoredProcedure [dbo].[GetVideosByArtistUrlSafeName]    Script Date: 5/14/2015 1:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[GetVideosByArtistUrlSafeName](@urlSafeName nvarchar(max))
as
	select  
		video.ISRC, video.Title, video.UrlSafeTitle 
		
	from Video 	 inner join Artist on artist.ArtistId = Video.ArtistId and Artist.UrlSafeName = @urlSafeName
 




GO
/****** Object:  StoredProcedure [dbo].[ListArtists]    Script Date: 5/14/2015 1:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[ListArtists]
as 
	select * from Artist;


GO
/****** Object:  StoredProcedure [dbo].[UpdateArtistById]    Script Date: 5/14/2015 1:52:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[UpdateArtistById](@artistId int, @newName nvarchar(50), @newUrlSafeName nvarchar(100))
as
	update dbo.Artist
	set Name = @newName, UrlSafeName = @newUrlSafeName
	where ArtistId = @artistId
select * from Artist where artistId = @artistId





GO
USE [master]
GO
ALTER DATABASE [VevoNew] SET  READ_WRITE 
GO

USE [VevoNew]
GO

/****** Object:  Table [dbo].[SampleData]    Script Date: 5/14/2015 11:22:37 AM ******/
DROP TABLE [dbo].[SampleData]
GO

/****** Object:  Table [dbo].[SampleData]    Script Date: 5/14/2015 11:22:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SampleData](
	[ISRC] [varchar](500) NULL,
	[Title] [varchar](500) NULL,
	[UrlSafeTitle] [varchar](500) NULL,
	[Artist1Name] [varchar](500) NULL,
	[Artist1UrlSafeName] [varchar](500) NULL,
	[ArtistId] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



SET ANSI_PADDING OFF
GO

INSERT INTO [dbo].[SampleData] 
([ISRC] ,[Title],[UrlSafeTitle] ,[Artist1Name],[Artist1UrlSafeName],[ArtistId])
VALUES (         
'USSM21200785' , 'Windows Down' , 'windows-down' , 'Big Time Rush' , 'big-time-rush' , '10')

INSERT INTO [dbo].[SampleData] 
([ISRC] ,[Title],[UrlSafeTitle] ,[Artist1Name],[Artist1UrlSafeName],[ArtistId])
VALUES (
'USRV81400102' , 'La La La (Brazil 2014)' , 'la-la-la-(brazil-2014)' , 'Shakira' , 'shakira' , '11')

INSERT INTO [dbo].[SampleData] 
([ISRC] ,[Title],[UrlSafeTitle] ,[Artist1Name],[Artist1UrlSafeName],[ArtistId])
VALUES (  
'USRV81400057' , 'We Are One (Ole Ola) [The Official 2014 FIFA World Cup Song] (Olodum Mix)' , 'we-are-one-(ole-ola)-[the-official-2014-fifa-world-cup-song]-(olodum-mix)' , 'Pitbull' , 'pitbull' , '12')


INSERT INTO [dbo].[SampleData] 
([ISRC] ,[Title],[UrlSafeTitle] ,[Artist1Name],[Artist1UrlSafeName],[ArtistId])
VALUES ( 
'GBUV71400428' , 'Bailando (Español)' , 'bailando-(español)' , 'Enrique Iglesias' , 'enrique-iglesias' , '13')

INSERT INTO [dbo].[SampleData] 
([ISRC] ,[Title],[UrlSafeTitle] ,[Artist1Name],[Artist1UrlSafeName],[ArtistId])
VALUES (
'USUV71400083' , 'Dark Horse (Official)' , 'dark-horse-(official)' , 'Katy Perry' , 'katy-perry' , '14')

INSERT INTO [dbo].[SampleData] 
([ISRC] ,[Title],[UrlSafeTitle] ,[Artist1Name],[Artist1UrlSafeName],[ArtistId])
VALUES ( 
'GB1101400141' , 'Summer' , 'summer' , 'Calvin Harris' , 'calvin-harris' , '15')

INSERT INTO [dbo].[SampleData] 
([ISRC] ,[Title],[UrlSafeTitle] ,[Artist1Name],[Artist1UrlSafeName],[ArtistId])
VALUES ( 
'USUV71301101' , 'Counting Stars' , 'counting-stars' , 'One Republic' , 'one-republic' , '16')

INSERT INTO [dbo].[SampleData] 
([ISRC] ,[Title],[UrlSafeTitle] ,[Artist1Name],[Artist1UrlSafeName],[ArtistId])
VALUES ( 
'USSM21302088' , 'All of Me' , 'all-of-me' , 'John Legend' , 'john-legend' , '17')

GO

delete from dbo.Video;
delete from dbo.Artist;

SET IDENTITY_INSERT dbo.Artist ON 

insert into dbo.Artist
( ArtistId,Name,UrlSafeName)
select
     sampleData.ArtistId,
	 sampleData.[Artist1Name],
	 sampleData.[Artist1UrlSafeName]
from dbo.SampleData

SET IDENTITY_INSERT dbo.Artist OFF 
go



INSERT INTO DBO.Video
([ISRC],Title, urlSafeTitle, artistId)
select 
[ISRC],[Title],[UrlSafeTitle],artistId  from dbo.sampleData
