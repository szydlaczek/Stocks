Declare @CompanyId int
Declare @StockRow int
DECLARE @FromDate DATETIME2(0)
DECLARE @ToDate   DATETIME2(0)
SET @FromDate = '2000-01-01 08:22:13' 
SET @ToDate = '2015-03-05 17:56:31'
Declare @RandomDate DATETIME2(0)
Set @CompanyId = 1
Set @StockRow = 1
While @CompanyId <= 20
Begin
	Insert into dbo.Companies values (Concat('Company ', @CompanyId), CAST(1111111100+@CompanyId as varchar))
	While @StockRow<=20000 
			Begin
				DECLARE @Seconds INT = DATEDIFF(SECOND, @FromDate, @ToDate)
				DECLARE @Random INT = ROUND(((@Seconds-1) * RAND()), 0)				
				Set @RandomDate=DATEADD(SECOND, @Random, @FromDate)
				Insert Into dbo.Stock values (Rand()*100, @RandomDate ,@CompanyId)
				Set @StockRow=@StockRow+1		
			End
		Set @CompanyId = @CompanyId + 1
		Set @StockRow=1	
	End