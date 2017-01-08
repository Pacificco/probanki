-- ��������� ������ ������������
if exists(select 1 from sysobjects where name = N'AddBalance' and xtype='P') drop proc AddBalance
go
create proc AddBalance (
	@UserId int,
	@TariffId tinyint,	
	@Sum float,
	@Period nvarchar(25),
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
			Balance float not null
		) 
		
		insert into #tblUserInfo
			select TariffId, ForecastTries, ForecastBeginDate, ForecastEndDate, Balance
			from UsersForecastInfo where UserId = @UserId

		if not exists (select 1 from #tblUserInfo) begin
			if @trancount = 0
				rollback transaction
			return 2
		end

		-- ����, �� ������� ����� ����������� ��������� �����
		declare @endPeriodDate datetime
		select @endPeriodDate = GETDATE()
		
		declare @curEndPeriodDate datetime
		set @curEndPeriodDate = (select ForecastEndDate from #tblUserInfo)		
		
		if @curEndPeriodDate is null begin
			set @endPeriodDate = GETDATE()
		end else begin
			set @endPeriodDate = @curEndPeriodDate
		end
		
		set @endPeriodDate =
		case @Period
			when N'month' then
				DATEADD(MONTH, 1, @endPeriodDate)
			when N'quarter' then
				DATEADD(MONTH, 3, @endPeriodDate)
			when N'half' then 
				DATEADD(MONTH, 6, @endPeriodDate)
			when N'year' then 
				DATEADD(YEAR, 1, @endPeriodDate)
		end

		--���������� ���������, � ������� ������ ������� ������� ������������	
		declare @monthCount int
		set @monthCount = 		
		case @Period
			when N'month' then 1
			when N'quarter' then 3
			when N'half' then 6
			when N'year' then 12
		end
								
		declare @curTryCount int
		set @curTryCount = (select ForecastTries from #tblUserInfo)
				
		declare @tryCount int
		set @tryCount = @monthCount * @TariffId * 2 + @curTryCount
						
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
		update UsersForecastInfo 
		set TariffId = @TariffId,
			ForecastTries = @tryCount,
			ForecastEndDate = @endPeriodDate,
			Balance = @Sum
		where UserId = @UserId
		-- ��������� ������ � �����
		insert into UsersForecastInfoArchive(UserId,Balance,TariffId,ForecastTries,ForecastBeginDate,ForecastEndDate,IsTariffLetterSent,ReportDate,UserReportId)
			select UserId,Balance,TariffId,ForecastTries,ForecastBeginDate,ForecastEndDate,IsTariffLetterSent, GETDATE(),30 
			from UsersForecastInfo
			where UserId = @UserId
		-- ��������� ������ � ����� �������
		insert into UserBalanceHistory(UserId,Sum,Operation,ReportDate,ReportUserId,Comment) 
			values (@UserId,@Sum,1,GETDATE(),30,@Comment)
				
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