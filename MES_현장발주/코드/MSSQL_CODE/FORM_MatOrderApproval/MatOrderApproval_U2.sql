USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[_1JO_MM_MatOrderApproval_U2]    Script Date: 2023-09-04 오후 6:25:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		권문규
-- Create date: 2023-08-10
-- Description: 발주승인 창에서 승인 체크박스 체크(Modified) 시 실행되는 프로시져_1.
--              체크된 데이터들 받아와서 TB_MaterialOrder에 날려주고, 수량을 바꿨을 수 있으므로 TB_OrderRequestList에 업데이트.
-- =============================================
-- 수정        : 2023-09-01
-- Description : 그리드에 보이지 않는 발주장소 컬럼(O/W) 추가 및 MaterialOrder에 삽입할 때 받아온 발주장소 데이터 넣어줄 수 있게 수정
-- ==============================================
ALTER PROCEDURE [dbo].[_1JO_MM_MatOrderApproval_U2]
	@PLANTCODE VARCHAR(10) , -- 공장				
	@ITEMCODE  VARCHAR(30) , -- 품목
	@POQTY     INT         , -- 발주승인수량
	@UNITCODE  VARCHAR(10) , -- 단위
	@CUSTNAME  VARCHAR(100), -- 거래처명
	@DATE      DATETIME,     -- 요청일시
	@MAKER     VARCHAR(10) , -- 승인자
	@OW        VARCHAR(1)  , -- 발주장소(현장:W / 사무실:O)

	@LANG    VARCHAR(10),
	@RS_CODE VARCHAR(1) OUTPUT,
	@RS_MSG  VARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @LD_NOWDATE DATETIME
	       ,@LS_NOWDATE VARCHAR(10)
		   ,@LI_POSEQ   INT
		   ,@LS_PONO    VARCHAR(20)
		   ,@LS_CUSTCODE VARCHAR(10)
		   ,@TEMP_DATE VARCHAR
	SET @LD_NOWDATE = GETDATE()
	SET @LS_NOWDATE = CONVERT(VARCHAR, @LD_NOWDATE, 23)

	SELECT @LS_CUSTCODE = CUSTCODE
	  FROM TB_CustMaster WITH(NOLOCK)
	 WHERE CUSTNAME = @CUSTNAME

	IF (SELECT ApprSTATUS
	      FROM TB_OrderRequestList WITH(NOLOCK)
		 WHERE MAKEDATE = @DATE) <> 'Y'
	BEGIN
		 -- TB_MaterialOrder에 Insert..!
		SELECT @LI_POSEQ = ISNULL(MAX(POSEQ), 0) + 1
		  FROM TB_MaterialOrder WITH(NOLOCK)
		 WHERE PLANTCODE = @PLANTCODE
		   AND PODATE    = @LS_NOWDATE

		SET @LS_PONO = 'PO' + CONVERT(VARCHAR, @LD_NOWDATE, 112) + RIGHT('00000' + CONVERT(VARCHAR, @LI_POSEQ), 4)

		INSERT INTO TB_MaterialOrder (PLANTCODE,  POSEQ,     PODATE,      PONO,     ITEMCODE,  POQTY,  UNITCODE,  CUSTCODE,     MAKEDATE,    MAKER,  AORDERSTATUS, [O/W])
						      VALUES (@PLANTCODE, @LI_POSEQ, @LS_NOWDATE, @LS_PONO, @ITEMCODE, @POQTY, @UNITCODE, @LS_CUSTCODE, @LD_NOWDATE, @MAKER, 'N', @OW)

		-- TB_OrderRequestList에 Update..!
		 UPDATE TB_OrderRequestList
		    SET ReqQTY     = @POQTY
			   ,ApprSTATUS = 'Y'
			   ,EDITOR     = @MAKER
			   ,EDITDATE   = @LD_NOWDATE
			   ,PONO       = @LS_PONO
		  WHERE MAKEDATE   = @DATE

		SET @RS_CODE = 'S'
	END
	ELSE
	BEGIN
		SET @RS_CODE = 'X'
		SET @RS_MSG  = '이미 발주 승인이 된 품목입니다. 확인 후 다시 시도해주세요.'
	END
END