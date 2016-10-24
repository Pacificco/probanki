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
	if @Nic is not null
		select @sqlWhere = @sqlWhere + ' and u.Nic like ''' + @Nic + '%'''
	if @Name is not null
		select @sqlWhere = @sqlWhere + ' and u.Name like ''' + @Name + '%'''
	if @Email is not null
		select @sqlWhere = @sqlWhere + ' and u.Email like ''' + @Email + '%'''
	
	select @sqlSelect = 'select count(1)'	
	select @sqlSelect = @sqlSelect + ' from Users u'	
	
	if @sqlWhere <> '' begin
		select @sqlWhere = SUBSTRING(@sqlWhere,6,LEN(@sqlWhere) - 5)
		select @sqlSelect = @sqlSelect + ' where ' + @sqlWhere
	end;
				
	exec (@sqlSelect)
end
go