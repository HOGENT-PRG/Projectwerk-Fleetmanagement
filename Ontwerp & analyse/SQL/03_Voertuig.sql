/****** Object:  Table [dbo].[Voertuig]    Script Date: 22/10/2021 19:47:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Voertuig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Merk] [varchar](50) NOT NULL,
	[Model] [varchar](50) NOT NULL,
	[Nummerplaat] [varchar](50) NOT NULL,
	[Chasisnummer] [varchar](250) NOT NULL,
	[Brandstof] [varchar](50) NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Kleur] [varchar](50) NULL,
	[AantalDeuren] [varchar](50) NULL
 CONSTRAINT [PK_Voertuig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


