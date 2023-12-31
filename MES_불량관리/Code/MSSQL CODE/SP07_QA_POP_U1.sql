USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_POP_U1]    Script Date: 2023-08-31 오전 2:24:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		조해찬
-- Create date: 2023-06-28
-- Description:	입력시 새로운 테이블 형성 및 로트연결
-- =============================================
ALTER PROCEDURE [dbo].[SP07_QA_POP_U1]
	@PLANTCODE      VARCHAR(30)
   ,@WORKCENTERCODE VARCHAR(30)
   ,@CHKCODE        VARCHAR(30)
   ,@MATLOTNO       VARCHAR(30)
   ,@ITEMCODE       VARCHAR(30)
   ,@ITEMNAME       VARCHAR(30)
   ,@STOCKQTY       INT
   ,@MAKER	        VARCHAR(30)

   ,@LANG    VARCHAR(10) = 'KO'
   ,@RS_CODE VARCHAR(1) OUTPUT
   ,@RS_MSG  VARCHAR(100) OUTPUT
AS
BEGIN
IF((SELECT PASSNO
     FROM P_Lot
	WHERE CHKCODE = @CHKCODE
	  AND C_STATE = '합')-@STOCKQTY <0)
BEGIN
	SET @RS_CODE = 'E'
	SET @RS_MSG = '남은 양보다 입력 양이 많습니다. 다시 진행해주세요'
	RETURN
END

DECLARE @LS_NOWDATE  VARCHAR(10)
       ,@LD_NOWDATE  DATETIME
	   ,@LS_LOTNO    VARCHAR(30)
	   ,@LI_INOUTSEQ INT
	   ,@LS_ORDERNO  VARCHAR(30)
	   ,@LI_INQTY    INT
	   ,@LS_ITEMCODE VARCHAR(30)

	SET @LD_NOWDATE = GETDATE()
	SET @LS_NOWDATE = CONVERT(VARCHAR,@LD_NOWDATE,23)
	SET @LS_LOTNO = 'LT_P' + REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR,GETDATE(),121),'-',''),':',''),' ',''),'.','')


-- P_FINALLOT 테이블에 입력하는 것

	INSERT INTO P_FINALLOT(PLANTCODE,  CHKCODE,   WORKCENTERCODE,  MATLOTNO,  FINALLOTNO,
	                       ITEMCODE,   ITEMNAME,  STOCKQTY,        UNITCODE,  MAKER,      MAKEDATE)
	                VALUES(@PLANTCODE, @CHKCODE,  @WORKCENTERCODE, @MATLOTNO, @LS_LOTNO,
					       @ITEMCODE,  @ITEMNAME, @STOCKQTY,       'EA',      @MAKER,     @LD_NOWDATE)

-- P_StockPP 테이블에 들어가는 것
	INSERT INTO TB_StockPP(PLANTCODE,   LOTNO,       ITEMCODE,   WHCODE,   STOCKQTY, 
						   INDATE,      MAKEDATE,    MAKER,      UNITCODE)
                    VALUES(@PLANTCODE,  @LS_LOTNO,   @ITEMCODE,  'WH003',  @STOCKQTY,
						   @LS_NOWDATE, @LD_NOWDATE, @MAKER,     'EA')	

-- P_StockPPrec 테이블에 들어가는 것

SELECT @LI_INOUTSEQ = ISNULL(MAX(INOUTSEQ),0) +1
  FROM TB_StockPPrec
 WHERE RECDATE = @LS_NOWDATE


	INSERT INTO TB_StockPPrec(PLANTCODE,  INOUTSEQ,     RECDATE,     LOTNO,     ITEMCODE,    WHCODE,
							  INOUTFLAG,  INOUTCODE,    INOUTQTY,    UNITCODE,  MAKEDATE,    MAKER)
                       VALUES(@PLANTCODE, @LI_INOUTSEQ, @LS_NOWDATE, @LS_LOTNO, @ITEMCODE,   'WH003',
					          'I',        '77',         @STOCKQTY,   'EA',      @LD_NOWDATE, @MAKER)

-- P_LOTTRACKING 테이블에 들어가는 것
SELECT DISTINCT @LS_ORDERNO = ORDERNO 
           FROM TP_WorkcenterPerProd 
          WHERE WORKCENTERCODE = @WORKCENTERCODE
            AND INLOTNO = @MATLOTNO
            AND ITEMCODE = @ITEMCODE

SELECT @LI_INQTY = COMPONENTQTY * @STOCKQTY
  FROM TB_BomMaster
 WHERE ITEMCODE = @ITEMCODE



SELECT DISTINCT @LS_ITEMCODE = ITEMCODE
  FROM TB_StockMMrec
 WHERE MATLOTNO = @MATLOTNO

																		       
	INSERT INTO TP_LotTracking(PLANTCODE,  LOTNO,     SEQ,          ORDERNO,      WORKCENTERCODE,  ITEMCODE,    PRODQTY, 
	                           UNITCODE,   CLOTNO,    CITEMCODE,    INQTY,        CUNITCODE,       MAKEDATE,    MAKER)
						VALUES(@PLANTCODE, @LS_LOTNO, '1',          @LS_ORDERNO , @WORKCENTERCODE, @ITEMCODE,   @STOCKQTY,
							  'EA',        @MATLOTNO, @LS_ITEMCODE, @LI_INQTY,    'EA',            @LD_NOWDATE, @MAKER)

-- P_LOT 테이블에 업데이트 할 것
	UPDATE P_Lot
	   SET PASSNO = PASSNO - @STOCKQTY
	 WHERE CHKCODE = @CHKCODE
	   AND C_STATE = '합'
END
