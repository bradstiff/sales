create table Country
(
	Id int not null identity,
	Name varchar(50) not null
	constraint PK_Country primary key (Id),
)

create table Proposal
(
	Id int not null identity,
	Name varchar(100) not null,
	ClientName varchar(100) not null,
	Comments varchar(max) null,
	ProductModelVersionId int not null,
	PriceModelVersionId int not null,
	ProductTypeIds bigint not null,
	constraint PK_Proposal primary key (Id),
)

create table ProposalCountry
(
	Id int not null identity,
	ProposalId int not null,
	CountryID int not null,
	ProductTypeIds bigint not null,
	constraint PK_ProposalCountry primary key (Id),
	constraint FK_ProposalCountry_Proposal foreign key (ProposalId) references Proposal (Id),
	constraint FK_ProposalCountry_Country foreign key (CountryId) references Country (Id),
)

create table PayrollProduct
(
	Id int not null identity,
	ProposalId int not null,
	Reporting bit null,
	PayslipStorage bit null,
	constraint PK_PayrollProduct primary key (Id),
	constraint FK_PayrollProduct_Proposal foreign key (ProposalId) references Proposal (Id),
)

create table PayrollProductCountry
(
	Id int not null identity,
	ProposalId int not null,
	CountryID int not null,
	LevelId smallint null,
	WeeklyPayees int null,
	BiWeeklyPayees int null,
	SemiMonthlyPayees int null,
	MonthlyPayees int null,
	Reporting bit null,
	PayslipStorage bit null,
	constraint PK_PayrollProductCountry primary key (Id),
	constraint FK_PayrollProductCountry_Proposal foreign key (ProposalId) references Proposal (Id),
)

create table HrProduct
(
	Id int not null identity,
	ProposalId int not null,
	LevelId smallint null,
	constraint PK_HrProduct primary key (Id),
	constraint FK_HrProduct_Proposal foreign key (ProposalId) references Proposal (Id),
)

create table HrProductCountry
(
	Id int not null identity,
	ProposalId int not null,
	CountryID int not null,
	LevelId smallint null,
	constraint PK_HrProductCountry primary key (Id),
	constraint FK_HrProductCountry_Proposal foreign key (ProposalId) references Proposal (Id),
)