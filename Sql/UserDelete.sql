-- Удаляет пользователя
if exists(select 1 from sysobjects where name = N'UserDelete' and xtype='P') drop proc UserDelete
go
create proc UserDelete (
	@UserId int
	) as
begin	

	set nocount on

	declare @trancount int
	select @trancount = @@TRANCOUNT

	begin try 

		if @trancount = 0
			begin transaction
				
		delete from ForecastsUsers where UserId = @UserId
		delete from UsersForecastInfo where UserId = @UserId
		delete from UsersForecastInfoArchive where UserId = @UserId
		delete from UsersTariffRequests where UserId = @UserId
		delete from UserBalanceHistory where UserId = @UserId
		delete from Users where Id = @UserId
			
		if @trancount = 0
			commit transaction			
			
		return 0
	
	end try 
	begin catch 
		if @trancount = 0
			rollback transaction
		return 1
	end catch
	
end
go