/****** Object:  Table [dbo].[Bestuurder]    Script Date: 22/10/2021 19:46:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Bestuurder](
	[Id] [int] IDENTITY(1,1) NOT NULL,  --<< auto increment bij alle tabellen toegevoegd
	[Naam] [varchar](75) NOT NULL,
	[Voornaam] [varchar](75) NOT NULL,
	[Geboortedatum] [date] NOT NULL,
	[AdresId] [int] NULL,
	[Rijksregisternummer] [varchar](11) NOT NULL, --<< veld uit klasse nog
	[Rijbewijssoort] [varchar](30) NOT NULL, --<< veld uit klasse nog
	[VoertuigId] [int] NULL, --<< in de plaats van VoertuigBestuurder tabel
	[TankkaartId] [int] NULL --<< in de plaats van TankkaartBestuurder tabel
 CONSTRAINT [PK_Bestuurder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bestuurder]  WITH CHECK ADD  CONSTRAINT [FK_Bestuurder_Adres] FOREIGN KEY([AdresId])
REFERENCES [dbo].[Adres] ([Id])
GO

ALTER TABLE [dbo].[Bestuurder] CHECK CONSTRAINT [FK_Bestuurder_Adres]
GO

-- extra FK constraint voor voertuigid
ALTER TABLE [dbo].[Bestuurder]  WITH CHECK ADD  CONSTRAINT [FK_Bestuurder_Voertuig] FOREIGN KEY([VoertuigId])
REFERENCES [dbo].[Voertuig] ([Id])
GO

ALTER TABLE [dbo].[Bestuurder] CHECK CONSTRAINT [FK_Bestuurder_Voertuig]
GO

-- extra FK constraint voor tankkaartid
ALTER TABLE [dbo].[Bestuurder]  WITH CHECK ADD  CONSTRAINT [FK_Bestuurder_Tankkaart] FOREIGN KEY([TankkaartId])
REFERENCES [dbo].[Tankkaart] ([Id])
GO

ALTER TABLE [dbo].[Bestuurder] CHECK CONSTRAINT [FK_Bestuurder_Tankkaart]
GO


