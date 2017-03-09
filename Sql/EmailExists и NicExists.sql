-- Проверка Email пользователя на существование в базе данных
if exists(select 1 from sysobjects where name = N'MailExists' and xtype='P') drop proc MailExists
go
create proc MailExists (
	@UserId int,
	@Email nvarchar(50)
	) as
begin
	
	SET NOCOUNT ON;

	begin try 
		if @UserId is null begin
			if exists(select 1 from Users where Lower(Email) = Lower(@Email))
				select 1 as returnResult
			else
				select 0 as returnResult
		end else begin
			if exists(select 1 from Users where Lower(Email) = Lower(@Email) and Id <> @UserId)
				select 1 as returnResult
			else
				select 0 as returnResult
		end
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
	@UserId int,
	@Nic nvarchar(50)
	) as
begin
	
	SET NOCOUNT ON;

	begin try 
		if @UserId is null begin
			if exists(select 1 from Users where Lower(Nic) = Lower(@Nic))
				select 1 as returnResult
			else
				select 0 as returnResult
		end else begin
			if exists(select 1 from Users where Lower(Nic) = Lower(@Nic) and Id <> @UserId)
				select 1 as returnResult
			else
				select 0 as returnResult
		end
	end try 
	begin catch 
		select 1 as returnResult
	end catch
    
end
go
