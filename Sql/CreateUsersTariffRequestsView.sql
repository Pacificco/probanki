-- Создает заявку на оформление Подписке
if exists(select 1 from sysobjects where name = N'CreateUsersTariffRequestsView' and xtype='P') drop proc CreateUsersTariffRequestsView
go
create proc CreateUsersTariffRequestsView (
	@Id				int,
	@UserId			int,
	@TariffId		tinyint,
	@PeriodId		varchar,
	@Sum			float,
	@IsPaid			bit,
	@PaidDate		datetime,
	@PaymentInfo	nvarchar(500),
	--@IsArchive		bit,
	--@ArchiveDate	datetime,
	--@CreateDate		datetime,
	--@Returned		bit,
	--@ReturnedDate	datetime,
	@Operation		int
) as	
begin	

	SET NOCOUNT ON;

	declare @trancount int
	select @trancount = @@TRANCOUNT

	begin try 	
	
		if @trancount = 0
			begin transaction
	
		if @Operation = 1 begin		-- Создаем заявку
		
			insert UsersTariffRequests(UserId,TariffId,PeriodId,Sum,IsPaid,PaidDate,PaymentInfo,IsArchive,ArchiveDate,CreateDate,Returned,ReturnedDate)
			values(@UserId,@TariffId,@PeriodId,@Sum,0,null,'',0,null,GETDATE(),0,null)
			
		end else 
		if @Operation = 2 begin		-- Проставляем оплату
		
			update UsersTariffRequests
			set IsPaid = @IsPaid, PaidDate = @PaidDate, PaymentInfo = @PaymentInfo
			where Id = @Id
		
		end else 
		if @Operation = 3 begin		-- Отменяем заявку
		
			update UsersTariffRequests
			set Returned = 1, ReturnedDate = GETDATE()
			where Id = @Id
		
		end else
		if @Operation = 4 begin		-- Архивируем заявку
		
			update UsersTariffRequests
			set IsArchive = 1, ArchiveDate = GETDATE()
			where Id = @Id
		
		end
	
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