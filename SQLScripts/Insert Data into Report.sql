USE [FamilyFinance]
GO

INSERT INTO [dbo].[Report]
           ([Id]
           ,[Name]
           ,[Description])
     VALUES
           (1, 'CostsByPeriod', '������� �� ������'),
		   (2, 'CostsByCategories', '������� �� ����������'),
		   (3, 'CostsByShops', '������� � ������� ���������'),
		   (4, 'CostsByPayTypes', '������� � ������� ����� �����'),
		   (5, 'CurrentBalance', '������� ������')
GO

