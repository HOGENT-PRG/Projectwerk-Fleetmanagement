/****** Object:  Table [dbo].[Adres]    Script Date: 22/10/2021 19:46:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Adres](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Straatnaam] [varchar](150) NOT NULL,
	[Huisnummer] [varchar](50) NOT NULL,
	[Postcode] [varchar](50) NOT NULL,
	[Plaatsnaam] [varchar](150) NOT NULL,
	[Provincie] [varchar](150) NOT NULL,
	[Land] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Adres] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


