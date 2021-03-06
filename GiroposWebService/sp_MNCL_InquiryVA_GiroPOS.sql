USE [MNC_GUI_SSS]
GO
/****** Object:  StoredProcedure [dbo].[sp_MNCL_InquiryVA_GiroPOS]    Script Date: 14/02/2022 14:29:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_MNCL_InquiryVA_GiroPOS]
	@VA_NUMB VARCHAR(50),

	@REKGIRO VARCHAR(50) OUT,
	@NOMINAL DECIMAL OUT,
	@ADMINFEE DECIMAL OUT,
	@NAMA VARCHAR(100) OUT
AS
BEGIN

	DECLARE @LSAGREE VARCHAR(20)
	DECLARE @InstAmt NUMERIC(17,2)
	DECLARE @lcAmt NUMERIC(17,2)
	DECLARE @tAmt NUMERIC(17,2)

	DECLARE @strError VARCHAR(50) = ''
	DECLARE @strErrorCode VARCHAR(50) = 'ERROR '
	DECLARE @strErrorMsg VARCHAR(2000) = ''

	--GET VA Number
	SELECT 
		@NAMA = SC.NAME,
		@LSAGREE = LA.LSAGREE,
		@REKGIRO = LA.LSAGREE
	FROM LS_AGREEMENT LA 
	JOIN SYS_CLIENT SC ON SC.CLIENT =  LA.LESSEE
	INNER JOIN PAYMENT_POINT_VA VA ON LA.LSAGREE = VA.LSAGREE AND VA.PAYMENT_POINT_CODE = 'GIROPOS'
	WHERE 1=1
		AND VA.VIRTUAL_ACCNO = @VA_NUMB

	IF (@LSAGREE IS NULL)
		BEGIN
			SET @strErrorCode += '9614'
			SET @strErrorMsg = @strErrorCode + ': ' + @VA_NUMB + ' VA Number does not exist'
			RAISERROR(@strErrorMsg, 16, 1)
		END
	ELSE
		BEGIN
			--GET INSTALLMENT AMOUNT
			SELECT @InstAmt = ISNULL(SUM(PAYMENT),0) FROM LS_LEDGERRENTAL WHERE LSAGREE = @LSAGREE AND DUEDATE < GETDATE()

			--GET PENALTY AMOUNT
			SELECT @lcAmt =  ISNULL(SUM(DR_CR),0) FROM LS_LEDGERPENALTY WHERE LSAGREE = @LSAGREE
			IF(@lcAmt < 0)
			BEGIN
				SET @lcAmt = 0
			END

			SET @NOMINAL = @InstAmt + @lcAmt
			SET @ADMINFEE = 0

			INSERT INTO [dbo].[TRX_GIROPOS_LOG]
			   ([trx_type]
			   ,[no_va]
			   ,[nama]
			   ,[nominal]
			   ,[adminfee]
			   ,[ref_no]
			   ,[waktu_proses]
			   ,[CRE_BY]
			   ,[CRE_DT])
			VALUES
				('Inquiry',
				@VA_NUMB,
				@NAMA,
				@NOMINAL,
				@ADMINFEE,
				'',
				'',
				'system',
				GETDATE())
		END
	

END
