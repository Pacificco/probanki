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
	if @Nic is not null and @Name is not null and @Email is not null and @IsActive is not null
		select COUNT(1) from Users u where u.Nic like @Nic + '%' and u.Name like @Name + '%'
			and u.Email like @Email + '%' and u.IsActive = @IsActive
	else 
	if @Nic is not null and @Name is not null and @Email is not null
		select COUNT(1) from Users u where u.Nic like @Nic + '%' and u.Name like @Name + '%'
			and u.Email like @Email + '%'
	else 
	if @Nic is not null and @Name is not null and @IsActive is not null
		select COUNT(1) from Users u where u.Nic like @Nic + '%' and u.Name like @Name + '%'
			and u.IsActive = @IsActive
	else
	if @Nic is not null and @Email is not null and @IsActive is not null
		select COUNT(1) from Users u where u.Nic like @Nic + '%'
			and u.Email like @Email + '%' and u.IsActive = @IsActive
	else
	if @Name is not null and @Email is not null and @IsActive is not null
		select COUNT(1) from Users u where u.Name like @Name + '%'
			and u.Email like @Email + '%' and u.IsActive = @IsActive
	else
	if @Email is not null and @IsActive is not null
		select COUNT(1) from Users u where
			u.Email like @Email + '%' and u.IsActive = @IsActive
	else
	if @Nic is not null and @IsActive is not null
		select COUNT(1) from Users u where u.Nic like @Nic + '%'
			and u.IsActive = @IsActive
	else
	if @Nic is not null and @Name is not null
		select COUNT(1) from Users u where u.Nic like @Nic + '%' and u.Name like @Name + '%'			
	else
	if @Name is not null and @Email is not null
		select COUNT(1) from Users u where u.Name like @Name + '%'
			and u.Email like @Email + '%'
	else
	if @IsActive is not null
		select COUNT(1) from Users u where u.IsActive = @IsActive
	else
	if @Email is not null
		select COUNT(1) from Users u where u.Email like @Email + '%'
	else
	if @Nic is not null
		select COUNT(1) from Users u where u.Nic like @Nic + '%'
	else
	if @Name is not null
		select COUNT(1) from Users u where u.Name like @Name + '%'
	else
		select COUNT(1) from Users u
end
go