-- Возвращает количество пользователей по фильтру
if exists(select 1 from sysobjects where name = N'UsersCount' and xtype='P') drop proc UsersCount
go
create proc UsersCount (
	@Nic nvarchar(50),
	@Name nvarchar(50),
	@Email nvarchar(50),	
	@IsActive bit
	) as
begin
	declare @sqlSelect nvarchar(2800)
	declare @sqlWhere nvarchar(2800)
	
	set @sqlSelect = ''
	set @sqlWhere = ''

	if @isActive is not null
		select @sqlWhere = @sqlWhere + ' and u.IsActive = ' + case when @isActive = 1 then '1' else '0' end
	
	select @sqlSelect = 'Select t1.*  From(Select ROW_NUMBER() OVER(Order By Nic) as RowNumber,'
	select @sqlSelect = @sqlSelect + 'Id, Nic, Name, Enail, Sex, LastName, FatherName, IsActive, IsSubscribed, Rols, EmailConfirmed, RegistrationDate, IsBan, Comment'
	select @sqlSelect = @sqlSelect + ' from Users'		
	if @sqlWhere <> ''
		exec (@sqlSelect + ' where ' + @sqlWhere + ' order by u.Nic')
	else
		exec (@sqlSelect + ' order by u.Nic')
	
end
go
	
