/****** Object:  Table [dbo].[TankkaartBrandstof]    Script Date: 22/10/2021 19:47:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TankkaartBrandstof](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TankkaartId] [int] NOT NULL,
	[Brandstof] [varchar](75) NOT NULL
 CONSTRAINT [PK_TankkaartBrandstof] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TankkaartBrandstof]  WITH CHECK ADD  CONSTRAINT [FK_TankkaartBrandstof_Tankkaart] FOREIGN KEY([TankkaartId])
REFERENCES [dbo].[Tankkaart] ([Id])
GO

ALTER TABLE [dbo].[TankkaartBrandstof] CHECK CONSTRAINT [FK_TankkaartBrandstof_Tankkaart]
GO
--