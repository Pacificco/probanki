if exists(select 1 from sysobjects where name = N'UsersTariffRequests' and xtype='U') drop table UsersTariffRequests
go
create table UsersTariffRequests(
	Id				int				not null,
	UserId			int				not null,
	TariffId		tinyint			not null,
	PeriodId		varchar(20)		not null,
	Sum				float			not null,
	IsPaid			bit				not null,
	PaidDate		datetime		null,
	PaymentInfo		nvarchar(500)	not null,
	IsArchive		bit				not null,
	ArchiveDate		datetime		null,
	CreateDate		datetime		not null,
	Returned		bit				not null,
	ReturnedDate	datetime		null,
	primary key (Id)
)