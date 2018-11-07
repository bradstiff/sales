create sequence ProposalSeq as int start with 1 increment by 1
create sequence ProposalCountrySeq as int start with 1 increment by 1
create table Country
(
	Id int not null identity,
	Name varchar(50) not null
	constraint PK_Country primary key (Id),
)

create table Proposal
(
	Id int not null,
	Name varchar(100) not null,
	ClientName varchar(100) not null,
	Comments varchar(max) null,
	ProductModelVersionId int not null,
	PriceModelVersionId int not null,
	ProductTypeIds bigint not null,
	PayrollProductScope_Reporting bit null,
	PayrollProductScope_PayslipStorage bit null,
	HrProductScope_LevelId smallint null,
	constraint PK_Proposal primary key (Id),
)

create table ProposalCountry
(
	Id int not null,
	ProposalId int not null,
	CountryID int not null,
	ProductTypeIds bigint not null,
	PayrollProductScope_LevelId smallint null,
	PayrollProductScope_WeeklyPayees int null,
	PayrollProductScope_BiWeeklyPayees int null,
	PayrollProductScope_SemiMonthlyPayees int null,
	PayrollProductScope_MonthlyPayees int null,
	PayrollProductScope_Reporting bit null,
	PayrollProductScope_PayslipStorage bit null,
	HrProductScope_LevelId smallint null,
	HrProductScope_Users int null,
	constraint PK_ProposalCountry primary key (Id),
	constraint FK_ProposalCountry_Proposal foreign key (ProposalId) references Proposal (Id),
	constraint FK_ProposalCountry_Country foreign key (CountryId) references Country (Id),
)

