-- ��������� ������ ������������
if exists(select 1 from sysobjects where name = N'AddBalance' and xtype='P') drop proc AddBalance
go
create proc AddBalance (
	@UserId int,
	@TariffId tinyint,	
	@Sum float,
	@PeriodId int,
	@Comment nvarchar(255)
	) as
begin	

	set nocount on

	declare @trancount int
	select @trancount = @@TRANCOUNT

	begin try 

		if @trancount = 0
			begin transaction

		-- ���������� � ������������
		create table #tblUserInfo(
			TariffId tinyint not null,
			ForecastTries tinyint not null,
			ForecastBeginDate datetime null,
			ForecastEndDate datetime null,		
			Balance float not null,
			IsConfirmed bit null
		) 
		declare @userExists bit = 0
		if exists (select 1 from UsersForecastInfo where UserId = @UserId) begin
			set @userExists = 1
		end else begin
			set @userExists = 0
		end
		
		if @userExists = 1 begin
			insert into #tblUserInfo
				select TariffId, ForecastTries, ForecastBeginDate, ForecastEndDate, Balance, IsConfirmed
				from UsersForecastInfo where UserId = @UserId
		end		
						
		-- ����, �� ������� ����� ����������� ��������� �����		
		declare @curEndPeriodDate datetime
		if exists (select 1 from #tblUserInfo where IsConfirmed = 1 and ForecastEndDate > GETDATE()) begin			
			set @curEndPeriodDate = (select ForecastEndDate from #tblUserInfo)	
		end else begin			
			set @curEndPeriodDate = null
		end					

		declare @endPeriodDate datetime
		if @curEndPeriodDate is null begin
			set @endPeriodDate = GETDATE()
		end else begin
			set @endPeriodDate = @curEndPeriodDate
		end		
				
		set @endPeriodDate =
		case @PeriodId
			--when N'month' then
			when 1 then
				DATEADD(MONTH, 1, @endPeriodDate)
			--when N'quarter' then
			when 2 then
				DATEADD(MONTH, 3, @endPeriodDate)
			--when N'half' then 
			when 3 then 
				DATEADD(MONTH, 6, @endPeriodDate)
			--when N'year' then 
			when 4 then 
				DATEADD(YEAR, 1, @endPeriodDate)
		end
		
		--���������� ���������, � ������� ������ ������� ������� ������������	
		declare @monthCount int
		set @monthCount = 		
		case @PeriodId
			/*when N'month' then 1
			when N'quarter' then 3
			when N'half' then 6
			when N'year' then 12*/
			when 1 then 1
			when 2 then 3
			when 3 then 6
			when 4 then 12
		end
		
		declare @fCount int
		set @fCount = 		
		case @TariffId
			when 1 then 4
			when 2 then 3
			when 3 then 2
			when 4 then 1
		end
		
		declare @curTryCount int
		if exists (select 1 from #tblUserInfo where IsConfirmed = 1 and ForecastEndDate > GETDATE()) begin
			set @curTryCount = (select ForecastTries from #tblUserInfo)
		end else begin
			set @curTryCount = 0
		end
				
		declare @tryCount int
		set @tryCount = @monthCount * @fCount * 2 + @curTryCount
				
		-- ���������� ������� �� ������� ������� �������� � ������ �� �����
		/*declare @curTariffId datetime
		set @curTariffId = (select TariffId from #tblUserInfo)
		declare @solution datetime
		set @solution = (select Solution from TariffMoveSolutions where CurrentTariffId = @curTariffId and NewTariffId = @TariffId)*/
		
		/*set @tryCount = @tryCount +
		case @solution
			when 0 then @curTryCount + @tryCount
			when 1 then CEILING(@curTryCount / 2) 
			when 2 then CEILING(@curTryCount / 3) 
			when 3 then CEILING(@curTryCount / 4) 
			when -3 then CEILING(@curTryCount * 4)
			when -2 then CEILING(@curTryCount * 4)
			when -1 then CEILING(@curTryCount * 4)
			else -@tryCount - 1
		end */
								
		-- ��������� ������ � ������� UsersForecastInfo
		if @userExists = 1 begin
			update UsersForecastInfo 
			set TariffId = @TariffId,
				ForecastTries = @tryCount,
				ForecastEndDate = @endPeriodDate,
				Balance = @Sum,
				ReportDate = GETDATE(),
				IsConfirmed = 0
			where UserId = @UserId
		end else begin
			insert UsersForecastInfo (UserId, TariffId, PeriodId, ForecastTries, ForecastBeginDate, ForecastEndDate, Balance, IsTariffLetterSent, IsConfirmed, UserReportId, ReportDate)
			values (@UserId, @TariffId, @PeriodId, @tryCount, GETDATE(), @endPeriodDate, @Sum, 0, 0, 30, GETDATE())
		end
		
		-- ��������� ������ � �����
		insert into UsersForecastInfoArchive(UserId,Balance,TariffId,PeriodId,ForecastTries,ForecastBeginDate,ForecastEndDate,IsTariffLetterSent,IsConfirmed,ReportDate,UserReportId)
			select UserId,Balance,TariffId,PeriodId,ForecastTries,case when ForecastBeginDate is null then getdate() else ForecastBeginDate end,ForecastEndDate,IsTariffLetterSent,IsConfirmed,GETDATE(),30 
			from UsersForecastInfo
			where UserId = @UserId
					
		-- ��������� ������ � ����� �������
		--insert into UserBalanceHistory(UserId,Balance,Operation,ReportDate,ReportUserId,Comment,TariffId,PeriodId) 
			--values (@UserId,@Sum,1,GETDATE(),30,@Comment,@TariffId,@PeriodId)		
		
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