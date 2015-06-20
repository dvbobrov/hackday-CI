grammar FcOutput;


/*
 * Parser Rules
 */

fcOutput : singleDiff+
;

singleDiff : COMP_FILES restOfLine NL
			 (singleDiffItem+ | NO_DIFF_FOUND NL)
;

singleDiffItem : STARS restOfLine NL
				 diffLineList
			     STARS restOfLine NL
			     diffLineList
			     STARS NL
;

diffLineList : diffLine*
;

diffLine : NUM COLON restOfLine NL
;

restOfLine : ~NL* 
;

/*
 * Tokens
 */

COMP_FILES : 'Comparing files';
STARS : '*****';
NUM : [0-9]+;
COLON : ':';
NO_DIFF_FOUND : 'FC: no differences encountered';
NL : ('\r'?'\n')+;
B : ('\t' | ' ') -> channel(HIDDEN);
