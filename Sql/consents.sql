/****** Object:  Table [dbo].[Consents]    Script Date: 1/13/2017 4:10:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consents](
	[Subject] [nvarchar](200) NOT NULL,
	[ClientId] [nvarchar](200) NOT NULL,
	[Scopes] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_dbo.Consents] PRIMARY KEY CLUSTERED 
(
	[Subject] ASC,
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
