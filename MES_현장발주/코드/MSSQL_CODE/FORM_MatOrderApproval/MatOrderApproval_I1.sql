USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[_1JO_MM_MatOrderApproval_I1]    Script Date: 2023-09-04 오후 6:25:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		권문규
-- Create date: 2023-08-14
-- Description: 발주승인 창에서 추가 버튼으로 새로운 데이터 생성 후 저장 시 실행되는 프로시저(ADD) -> 새로 발주 시 실행될 프로시저임.
-- =============================================
-- 수정        : 2023-09-01
-- Description : 그리드에 보이지 않는 발주장소 컬럼(O/W) 추가 및 MaterialOrder에 삽입할 때 받아온 발주장소 데이터 넣어줄 수 있게 수정
-- =============================================
ALTER PROCEDURE [dbo].[_1JO_MM_MatOrderApproval_I1]
	@PLANTCODE VARCHAR(10) , -- 공장
	@REQDATE   VARCHAR(15) , -- 요청일자
	@ITEMCODE  VARCHAR(30) , -- 품목
	@REQQTY    INT         , -- 발주수량
	@UNITCODE  VARCHAR(10) , -- 단위
	@CUSTNAME  VARCHAR(100), -- 거래처명
	@CHK       VARCHAR(10),
	@MAKER     VARCHAR(10) , -- 발주요청자
	@OW        VARCHAR(1),   -- 발주장소(현장:W / 사무실:O)

	@LANG    VARCHAR(10),
	@RS_CODE VARCHAR(1) OUTPUT,
	@RS_MSG  VARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @LD_NOWDATE DATETIME
	       ,@LS_NOWDATE VARCHAR(10)
		   ,@LI_REQSEQ   INT
		   ,@LS_CUSTCODE VARCHAR(10)
	SET @LD_NOWDATE = GETDATE()
	SET @LS_NOWDATE = CONVERT(VARCHAR, @LD_NOWDATE, 23)
	
	SELECT @LS_CUSTCODE = CUSTCODE
	  FROM TB_CustMaster WITH(NOLOCK)
	 WHERE CUSTNAME = @CUSTNAME
	
	 SELECT @LI_REQSEQ = ISNULL(MAX(ReqSEQ), 0) + 1
	   FROM TB_OrderRequestList
	  WHERE PLANTCODE = @PLANTCODE
	    AND ReqDATE   = @REQDATE

	IF (@CHK = '1') -- 새로운 행 생성과 동시에 체크해서 발주승인 하는경우
	BEGIN
		DECLARE @LI_POSEQ INT
		       ,@LS_PONO  VARCHAR(20)

		SELECT @LI_POSEQ = ISNULL(MAX(POSEQ), 0) + 1
		  FROM TB_MaterialOrder WITH(NOLOCK)
		 WHERE PLANTCODE = @PLANTCODE
		   AND PODATE    = @LS_NOWDATE

		SET @LS_PONO = 'PO' + CONVERT(VARCHAR, @LD_NOWDATE, 112) + RIGHT('00000' + CONVERT(VARCHAR,@LI_POSEQ),4)

		INSERT INTO TB_OrderRequestList (PLANTCODE,  ReqSEQ,     ReqDATE,   ITEMCODE,  ReqQTY,  UNITCODE,  CUSTCODE,     ApprSTATUS, [O/W], MAKER,  MAKEDATE,    PONO)
					             VALUES (@PLANTCODE, @LI_REQSEQ, @REQDATE,  @ITEMCODE, @REQQTY, @UNITCODE, @LS_CUSTCODE, 'Y',        @OW,   @MAKER, @LD_NOWDATE, @LS_PONO)

		INSERT INTO TB_MaterialOrder (PLANTCODE,  POSEQ,     PODATE,      PONO,     ITEMCODE,  POQTY,   UNITCODE,  CUSTCODE,     INFLAG, MAKEDATE,    MAKER,  AORDERSTATUS, [O/W])
		                      VALUES (@PLANTCODE, @LI_POSEQ, @LS_NOWDATE, @LS_PONO, @ITEMCODE, @REQQTY, @UNITCODE, @LS_CUSTCODE, 'N',    @LD_NOWDATE, @MAKER, 'N', @OW)
	END
	ELSE -- 새로운 행 생성해서 수량만 넣고 발주승인은 안하는경우.
	BEGIN
		INSERT INTO TB_OrderRequestList (PLANTCODE,  ReqSEQ,     ReqDATE,   ITEMCODE,  ReqQTY,  UNITCODE,  CUSTCODE,     ApprSTATUS, [O/W], MAKER,  MAKEDATE)
					             VALUES (@PLANTCODE, @LI_REQSEQ, @REQDATE,  @ITEMCODE, @REQQTY, @UNITCODE, @LS_CUSTCODE, 'N',        @OW,   @MAKER, @LD_NOWDATE)
	END

	SET @RS_CODE = 'S'
END
