CREATE TABLE [dbo].[TypesOfProsthetics] (
    [ProstheticsID]   INT           NOT NULL,
    [ProstheticsName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_TypeOfProsthetics] PRIMARY KEY CLUSTERED ([ProstheticsID] ASC)
);

