-- ��������� �������� �������������, � ������� ����������� ��������
if exists(select 1 from sysobjects where name = N'CheckUsersTariff' and xtype='P') drop proc CheckUsersTariff
go
create proc CheckUsersTariff as
begin
	
	SET NOCOUNT ON;

	declare @trancount int
	select @trancount = @@TRANCOUNT

	declare @users table(
		UserId		int		not null
	)

	begin try 	
	
		if @trancount = 0
			begin transaction
		
		declare @curDate datetime
		select @curDate = GETDATE()
		
		-- ���� �������������, � ������� ����������� ��������
		insert @users
		select i.UserId from UsersForecastInfo i where i.IsConfirmed = 1 and i.ForecastEndDate < @curDate
		
		-- ���� ����� ������������ ����, ���������� �� ������
		if exists(select 1 from @users) begin
		
			-- ��������� ������ � ������� UsersForecastInfo		
			update i
			set i.TariffId = 0, i.ForecastTries = 0, i.ForecastBeginDate = null, i.ForecastEndDate = null, i.IsTariffLetterSent = 0, 
				i.ReportDate = GETDATE(), i.UserReportId = 2, i.Balance = 0, i.IsConfirmed = 0
			from UsersForecastInfo i join @users u on i.UserId = u.UserId
			
			-- ��������� ������ � �����
			insert UsersForecastInfoArchive(UserId,TariffId,ForecastTries,ForecastBeginDate,ForecastEndDate,IsTariffLetterSent,ReportDate,UserReportId,Balance,IsConfirmed)
				select i.UserId,i.TariffId,i.ForecastTries,i.ForecastBeginDate,i.ForecastEndDate,i.IsTariffLetterSent,GETDATE(),i.UserReportId,i.Balance,i.IsConfirmed 
				from UsersForecastInfo i join @users u on i.UserId = u.UserId			
		
		end
		
		-- ��������� ������ �� ��������� ��������
		
		-- ���������� ��������� ������� �������������
		delete @users
		-- ���� �������������, � ������� ��� �������� ��������
		insert @users
		select i.UserId from UsersForecastInfo i where i.ForecastEndDate is null
		
		-- ���� ����� ������������ ����
		if exists(select 1 from @users) begin
			
			declare @requests table(
				UserId		int		not null,
				RequestId	int		not null
			)
			
			-- ���� �������� � ���������� ������ ��� ������������� ��� ��������
			insert @requests
			select r.UserId, r.Id from UsersTariffRequests r join @users u on r.UserId = u.UserId where r.IsArchive = 0 and r.IsPaid = 1
			
			-- ���� ����� ������ ����
			if exists(select 1 from @requests) begin
			
				-- ��������� ������ � ������� UsersForecastInfo
				update i
				set i.TariffId = r.TariffId, i.ForecastTries = 0, i.ForecastBeginDate = null, i.ForecastEndDate = null, i.IsTariffLetterSent = 0, 
					i.ReportDate = GETDATE(), i.UserReportId = 2, i.Balance = 0, i.IsConfirmed = 0
				from UsersForecastInfo i 
					join @requests u on i.UserId = u.UserId
					join UsersTariffRequests r on r.Id = u.RequestId
				
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

