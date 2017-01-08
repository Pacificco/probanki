-- Создание нового пользователя в базе данных
if exists(select 1 from sysobjects where name = N'CreateNewUser' and xtype='P') drop proc CreateNewUser
go
create proc CreateNewUser (
	@NicName nvarchar(20), 
	@Name nvarchar(20), 
	@Sex int,
	@Email nvarchar(50), 
	@Password nvarchar(32), 
	@Subscribed nvarchar(32),
	@Role nvarchar(255),
	@Token nvarchar(255)
	) as
begin
	
	SET NOCOUNT ON;

	declare @trancount int
	select @trancount = @@TRANCOUNT

	begin try 
	
		if @trancount = 0
			begin transaction
		
		--Создаем запись в таблице Users
		insert Users(Nic, Name, Sex, Email, Password, IsSubscribed, IsActive, Rols, Token)
		values (@NicName, @Name, @Sex, @Email, @Password, @Subscribed, 0, @Role, @Token)
		
		declare @newId int
		set @newId = SCOPE_IDENTITY()
		
		--Создаем запись в таблице UsersForecastInfo		
		insert UsersForecastInfo(UserId,TariffId,ForecastTries,ForecastBeginDate,ForecastEndDate,IsTariffLetterSent,ReportDate,UserReportId,Balance)
		values(@newId,0,0,null,null,0,GETDATE(),2,0)		
		
		if @trancount = 0
			commit transaction
		
		select @newId
	end try 
	begin catch 
		if @trancount = 0
			rollback transaction
			
		select 0 as returnResult
	end catch
    
end
go
