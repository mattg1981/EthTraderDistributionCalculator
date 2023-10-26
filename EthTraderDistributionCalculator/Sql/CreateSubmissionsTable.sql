USE [Tips]
GO

/****** Object:  Table [dbo].[DistributionSubmissions]    Script Date: 10/26/2023 9:19:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DistributionSubmissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubmissionId] [nvarchar](50) NULL,
	[Score] [int] NULL,
	[Author] [nvarchar](max) NULL,
	[Date] [datetime2](7) NULL,
	[Comments] [int] NULL,
	[Flair] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


