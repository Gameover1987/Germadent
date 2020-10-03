CREATE TABLE [dbo].[ConditionsOfProsthetics] (
    [ConditionID]   INT           IDENTITY (1, 1) NOT NULL,
    [ConditionName] NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_ConditionsOfProsthetics] PRIMARY KEY CLUSTERED ([ConditionID] ASC)
);

