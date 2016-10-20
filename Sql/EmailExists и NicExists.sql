-- Проверка Email пользователя на существование в базе данных
if exists(select 1 from sysobjects where name = N'MailExists' and xtype='P') drop proc MailExists
go
create proc MailExists (
	@Email nvarchar(50)
	) as
begin
	
	SET NOCOUNT ON;

	begin try 
		if exists(select 1 from Users where Lower(Email) = Lower(@Email))
			select 1 as returnResult
		else
			select 0 as returnResult
	end try 
	begin catch 
		select 1 as returnResult
	end catch
    
end
go

-- Проверка Никнейма пользователя на существование в базе данных
if exists(select 1 from sysobjects where name = N'NicExists' and xtype='P') drop proc NicExists
go
create proc NicExists (
	@Nic nvarchar(50)
	) as
begin
	
	SET NOCOUNT ON;

	begin try 
		if exists(select 1 from Users where Lower(Nic) = Lower(@Nic))
			select 1 as returnResult
		else
			select 0 as returnResult
	end try 
	begin catch 
		select 1 as returnResult
	end catch
    
end
go
