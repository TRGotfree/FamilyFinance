USE [FamilyFinance]
GO

INSERT INTO [dbo].[Report]
           ([Id]
           ,[Name]
           ,[Description])
     VALUES
           (1, 'CostsByPeriod', 'Расходы за период'),
		   (2, 'CostsByCategories', 'Расходы по категориям'),
		   (3, 'CostsByShops', 'Расходы в разрезе магазинов'),
		   (4, 'CostsByPayTypes', 'Расходы в разрезе видов оплат'),
		   (5, 'CurrentBalance', 'Текущий баланс')
GO

