<?php
//Block access from file
if(__FILE__ == $_SERVER['SCRIPT_FILENAME']){
    include_once $_SERVER['DOCUMENT_ROOT'].'/errors/403.html';
    die();
}
class Extensions{
    static function init (){

        //file_force_content is now globally accessible
        function file_force_contents( $fullPath, $contents = "", $flags = 0 ){
            $parts = explode( '\\', $fullPath );
            array_pop( $parts );
            $dir = implode( '\\', $parts );
           
            if( !is_dir( $dir ) )
                mkdir( $dir, 0777, true );
           
            file_put_contents( $fullPath, $contents, $flags );
        }

    }

}