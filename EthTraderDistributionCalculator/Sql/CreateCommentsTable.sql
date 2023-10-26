USE [Tips]
GO

/****** Object:  Table [dbo].[DistributionComments]    Script Date: 10/26/2023 9:18:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DistributionComments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CommentId] [nvarchar](50) NULL,
	[Score] [int] NULL,
	[Author] [nvarchar](max) NULL,
	[Date] [datetime2](7) NULL,
	[SubmissionId] [nvarchar](50) NULL,
	[IsFromDaily] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[DistributionComments] ADD  CONSTRAINT [DF_DistributionComments_IsFromDaily]  DEFAULT ((0)) FOR [IsFromDaily]
GO


