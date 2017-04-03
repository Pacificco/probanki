begin tran
exec AddBalance 36, 3, 100, 2, 'Тест1'
rollback tran


set IDENTITY_INSERT Users on 
insert into Users(
    [Id]
    ,[Name]
      ,[Email]
      ,[Password]
      ,[Sex]
      ,[LastName]
      ,[FatherName]
      ,[IsActive]
      ,[IsSubscribed]
      ,[Nic]
      ,[Rols]
      ,[EmailConfirmed]
      ,[Avatar]
      ,[Country]
      ,[RegionId]
      ,[City]
      ,[RegistrationDate]
      ,[LastVisitDate]
      ,[IsBan]
      ,[BanDate]
      ,[Comment]
      ,[Token]
      ,[Rang]
)
values(
    30,
    'Бот для автоматическ',
    'support@probanki.net',
    '',
    1,
    '',
    '',
    0,
    0,
    'ClubBot',
    'admin',
    0,
    NULL,
    NULL,
    NULL,
    NULL,
    getdate(),
    NULL,
    0,
    NULL,
    '',
    '',
    NULL
)
set IDENTITY_INSERT Users off 