use master
go

sp_configure 'clr_enabled', 1
go
reconfigure
go

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Range]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [Range]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Split]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [Split]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[StrConcat]'))
	DROP AGGREGATE [StrConcat]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[BitwiseOr]'))
	DROP AGGREGATE [BitwiseOr]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[BitwiseAnd]'))
	DROP AGGREGATE [BitwiseAnd]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Average]'))
	DROP AGGREGATE [Average]
GO

/****** Object:  SqlAssembly [SqlServer.Clr.Extensions]    Script Date: 02/26/2011 13:53:35 ******/
IF  EXISTS (SELECT * FROM sys.assemblies asms WHERE asms.name = N'SqlServer.Clr.Extensions' and is_user_defined = 1)
DROP ASSEMBLY [SqlServer.Clr.Extensions]
GO

/****** Object:  SqlAssembly [SqlServer.Clr.Extensions]    Script Date: 02/26/2011 13:53:35 ******/
CREATE ASSEMBLY [SqlServer.Clr.Extensions]
AUTHORIZATION [dbo]
FROM 'C:\Users\Jeroen\Documents\Visual Studio 2010\Projects\SqlServer.Clr.Extensions\SqlServer.Clr.Extensions\bin\Debug\SqlServer.Clr.Extensions.dll'
WITH PERMISSION_SET = SAFE
GO

CREATE FUNCTION [Range](@start bigint, @end bigint, @incr bigint)
  RETURNS TABLE(n bigint)
  AS EXTERNAL NAME [SqlServer.Clr.Extensions].[SqlServer.Clr.Extensions.Functions].[Range]
go


CREATE FUNCTION [Split](@input nvarchar(max), @separators nvarchar(100))
  RETURNS TABLE(n nvarchar(max))
  AS EXTERNAL NAME [SqlServer.Clr.Extensions].[SqlServer.Clr.Extensions.Functions].[Split]
go

CREATE AGGREGATE [StrConcat](@input nvarchar(max))
	RETURNS nvarchar(max)
	EXTERNAL NAME [SqlServer.Clr.Extensions].[SqlServer.Clr.Extensions.Aggregates.StrConcat]
GO

CREATE AGGREGATE [BitwiseOr](@input bigint)
	RETURNS bigint
	EXTERNAL NAME [SqlServer.Clr.Extensions].[SqlServer.Clr.Extensions.Aggregates.BitwiseOr]
GO

CREATE AGGREGATE [BitwiseAnd](@input bigint)
	RETURNS bigint
	EXTERNAL NAME [SqlServer.Clr.Extensions].[SqlServer.Clr.Extensions.Aggregates.BitwiseAnd]
GO

CREATE AGGREGATE [Average](@input decimal)
	RETURNS decimal
	EXTERNAL NAME [SqlServer.Clr.Extensions].[SqlServer.Clr.Extensions.Aggregates.Average]
GO


SELECT n from dbo.Range(1,10,2)

select n FROM Split('a,b,c', ',')

SELECT dbo.StrConcat(n) FROM (
	select 1 as x, n FROM Split('a,b,c', ',')
	) y
GROUP BY x

SELECT dbo.BitwiseOr(n) FROM (
	select 1 as x, convert(bigint, n) n FROM Split('3,4', ',')
	) y
GROUP BY x

SELECT dbo.BitwiseAnd(n) FROM (
	select 1 as x, convert(bigint, n) n FROM Split('1,3', ',')
	) y
GROUP BY x

SELECT dbo.Average(n) FROM (
	select 1 as x, convert(decimal, n) n FROM Split('3.5,4', ',')
	) y
GROUP BY x
