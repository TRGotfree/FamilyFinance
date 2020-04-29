SET IDENTITY_INSERT [dbo].[PayType] ON
INSERT INTO [dbo].[PayType] ([Id], [Name], [IsRemoved]) VALUES (1, N'Наличность', 0)
INSERT INTO [dbo].[PayType] ([Id], [Name], [IsRemoved]) VALUES (2, N'Пластиковая карта', 0)
INSERT INTO [dbo].[PayType] ([Id], [Name], [IsRemoved]) VALUES (3, N'PayMe', 0)
INSERT INTO [dbo].[PayType] ([Id], [Name], [IsRemoved]) VALUES (4, N'Click', 0)
SET IDENTITY_INSERT [dbo].[PayType] OFF
