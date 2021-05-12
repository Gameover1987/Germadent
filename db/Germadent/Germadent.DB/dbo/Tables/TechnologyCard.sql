﻿CREATE TABLE [dbo].[TechnologyCard] (
    [PricePositionID]       INT NOT NULL,
    [ProductID]             INT NOT NULL,
    [TechnologyOperationID] INT NOT NULL,
    CONSTRAINT [FK_TechnologyCard_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID]),
    CONSTRAINT [FK_TechnologyCard_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID]),
    CONSTRAINT [FK_TechnologyCard_TechnologyOperations] FOREIGN KEY ([TechnologyOperationID]) REFERENCES [dbo].[TechnologyOperations] ([TechnologyOperationID])
);
