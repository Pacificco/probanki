-- Обновление прогноза в базе данных
if exists(select 1 from sysobjects where name = N'EditForecast' and xtype='P') drop proc EditForecast
go
create proc EditForecast (
	@Id				int,
	@SubjectId		tinyint,	
	@WinAmount		float,
	@ForecastDate	datetime,	
	@UserId			int
	) as
begin
	
	set nocount on

	declare @trancount int
	select @trancount = @@TRANCOUNT

	begin try 
		
		if not exists (select 1 from Forecasts where Id = @Id) 
			return 1
		
		if @trancount = 0
			begin transaction
		
		update Forecasts 
		set 			
			SubjectId =	@SubjectId,			
			WinAmount =		@WinAmount,		
			ForecastDate =	@ForecastDate,			
			ReportDate =	GETDATE(),	
			ReportUserId =	@UserId
		where Id = @Id
		
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