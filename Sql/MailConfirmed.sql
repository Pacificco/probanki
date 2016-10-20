-- Проверка подтвержден ли Email
if exists(select 1 from sysobjects where name = N'MailConfirmed' and xtype='P') drop proc MailConfirmed
go
create proc MailConfirmed (
	@Email nvarchar(50),
	@Token nvarchar(255)
	) as
begin
	
	SET NOCOUNT ON;

	begin try 
		if exists(select 1 from Users where Lower(Email) = Lower(@Email) and Token = @Token and EmailConfirmed = 1)
			select 1 as returnResult
		else
			select 0 as returnResult
	end try 
	begin catch 
		select 1 as returnResult
	end catch
    
end
go