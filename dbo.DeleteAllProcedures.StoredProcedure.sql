U S E   [ F e e d b a c k 3 6 0 _ D e v 2 ]  
 G O  
 / * * * * * * *   O b j e c t :     S t o r e d P r o c e d u r e   [ d b o ] . [ D e l e t e A l l P r o c e d u r e s ]         S c r i p t   D a t e :   0 6 / 1 9 / 2 0 1 5   1 3 : 2 6 : 2 8   * * * * * * /  
 I F     E X I S T S   ( S E L E C T   *   F R O M   s y s . o b j e c t s   W H E R E   o b j e c t _ i d   =   O B J E C T _ I D ( N ' [ d b o ] . [ D e l e t e A l l P r o c e d u r e s ] ' )   A N D   t y p e   i n   ( N ' P ' ,   N ' P C ' ) )  
 D R O P   P R O C E D U R E   [ d b o ] . [ D e l e t e A l l P r o c e d u r e s ]  
 G O  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 I F   N O T   E X I S T S   ( S E L E C T   *   F R O M   s y s . o b j e c t s   W H E R E   o b j e c t _ i d   =   O B J E C T _ I D ( N ' [ d b o ] . [ D e l e t e A l l P r o c e d u r e s ] ' )   A N D   t y p e   i n   ( N ' P ' ,   N ' P C ' ) )  
 B E G I N  
 E X E C   d b o . s p _ e x e c u t e s q l   @ s t a t e m e n t   =   N ' c r e a t e   P r o c e d u r e   [ d b o ] . [ D e l e t e A l l P r o c e d u r e s ]  
 A s  
             d e c l a r e   @ p r o c N a m e   v a r c h a r ( 5 0 0 )  
             d e c l a r e   c u r   c u r s o r    
                         f o r   s e l e c t   [ n a m e ]   f r o m   s y s . o b j e c t s   w h e r e   t y p e   =   ' ' p ' '  
             o p e n   c u r  
    
  
             f e t c h   n e x t   f r o m   c u r   i n t o   @ p r o c N a m e  
             w h i l e   @ @ f e t c h _ s t a t u s   =   0  
             b e g i n  
                         i f   @ p r o c N a m e   < >   ' ' D e l e t e A l l P r o c e d u r e s ' '  
                                     e x e c ( ' ' d r o p   p r o c e d u r e   ' '   +   @ p r o c N a m e )  
                                     f e t c h   n e x t   f r o m   c u r   i n t o   @ p r o c N a m e  
             e n d  
             c l o s e   c u r  
             d e a l l o c a t e   c u r  
 '    
 E N D  
 G O  
 