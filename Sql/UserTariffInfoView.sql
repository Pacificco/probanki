-- ���������� ���������� � ������� ������������ �� ���� ������ �� ��������������
if exists(select 1 from sysobjects where name = N'UserTariffInfoView' and xtype='P') drop proc UserTariffInfoView
go
create proc UserTariffInfoView (
	@UserId int
	) as
begin	
	select * from UsersForecastInfo where UserId = @UserId    
end