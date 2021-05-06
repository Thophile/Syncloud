<?php
//Include classes
include_once $_SERVER['DOCUMENT_ROOT'].'/objects/Database.php';
include_once $_SERVER['DOCUMENT_ROOT'].'/objects/Request.php';
include_once $_SERVER['DOCUMENT_ROOT'].'/objects/Router.php';
include_once $_SERVER['DOCUMENT_ROOT'].'/objects/Token.php';
include_once $_SERVER['DOCUMENT_ROOT'].'/objects/Extensions.php';


//Include Environnement file
include_once $_SERVER['DOCUMENT_ROOT'].'/env.php';

//Instanciate objects
$db = new Database();
$router = new Router(new Request, $db);
Extensions::init();

//Prevent acces from file name
$router->get('/', function() {
  include_once $_SERVER['DOCUMENT_ROOT'].'/pages/portal.php';
  die();
});

$router->post('/register', function($request, $db) {
  $content = json_decode(file_get_contents("php://input"),true);

  $db->registerUser(
    $content['username'],
    $content['mail'],
    password_hash($content['password'], PASSWORD_DEFAULT));

});

$router->post('/login', function($request, $db) {
  //getting query content
  $content = json_decode(file_get_contents("php://input"),true);

  $user = $db->connectUser(
    $content['uid'],
    $content['password']
  );
  if($user != null)
  {
    http_response_code(200);
    echo Token::generate($user["username"]);
  }else{
    http_response_code(400);
  }
  die();
});

$router->get('/list', function($request){
	$token = explode(" ",apache_request_headers()['Authorization'])[1];
  
  if (isset($token) && $username = Token::validate($token)){
    $path = $_SERVER['DOCUMENT_ROOT'].'\\usersData\\'.$username;
	$ar = scandir($path,1);
    $list = json_encode(array_chunk($ar,count($ar)-2)[0]);

    echo $list;
    return;
  }
  
  http_response_code(403);
});

$router->get('/folder', function($request){
  $token = explode(" ",apache_request_headers()['Authorization'])[1];
  $foldername = $_GET["name"];
  
  if (isset($token) && $username = Token::validate($token)){
    $folder = $_SERVER['DOCUMENT_ROOT'].'\\usersData\\'.$username.'\\'.$foldername;
    
    if (is_dir($folder)){
      // Get real path for our folder
      $rootPath = $folder;
	  $zipname = $foldername.".zip";

      // Initialize archive object
      $zip = new ZipArchive();
      $zip->open($zipname, ZipArchive::CREATE | ZipArchive::OVERWRITE);

      // Create recursive directory iterator
      /** @var SplFileInfo[] $files */
      $files = new RecursiveIteratorIterator(
          new RecursiveDirectoryIterator($rootPath),
          RecursiveIteratorIterator::LEAVES_ONLY
      );

      foreach ($files as $name => $file)
      {
          // Skip directories (they would be added automatically)
          if (!$file->isDir())
          {
              // Get real and relative path for current file
              $filePath = $file->getRealPath();
              $relativePath = substr($filePath, strlen($rootPath) + 1);

              // Add current file to archive
              $zip->addFile($filePath, $relativePath);
          }
      }

      // Zip archive will be created only after closing object
      $zip->close();
      header('Content-Type: '.filetype($zipname));
      header('Content-disposition: attachment; filename='.$zipname);
      header('Content-Length: ' . filesize($zipname));
      readfile($zipname); 
	  unlink($zipname);
    }else{
		http_response_code(500);
	}

    return;
  }
  
  http_response_code(403);
});

$router->post('/folder', function($request){

  $token = explode(" ",apache_request_headers()['Authorization'])[1];
  if (isset($token) && $username = Token::validate($token)){    
    
    //Handle files if submitted
    if (isset($_FILES['file'])) {

      $filename = $_FILES['file']['name'];

      $path = $_SERVER['DOCUMENT_ROOT'].'\\usersData\\'.$username;
      if (!file_exists($path)) {
        mkdir($path, 0777, true);
      }

      $tmpFilePath = $_FILES['file']['tmp_name'];
      echo("TMP Path of files : ".$tmpFilePath."\n");

      if ($tmpFilePath != ""){
        //Upload the file from the tmp dir
        if(move_uploaded_file($tmpFilePath, $path."\\".$filename)){
			//unzip
			$zip = new ZipArchive;
	
			if ($zip->open($path."\\".$filename) === TRUE) {
				
				$zip->extractTo($path."\\".str_replace(".zip","",$filename));
				$zip->close();
				unlink($path."\\".$filename);
			}
        }

      }
    }else{
		echo("File too large");
	}
    return;
  }else{
    http_response_code(403);
  }

  
});

$router->post('/delete_folder', function($request){
	$content = json_decode(file_get_contents("php://input"),true);
	$token = explode(" ",apache_request_headers()['Authorization'])[1];

	if (isset($token) && $username = Token::validate($token)){  
		$folder = $_SERVER['DOCUMENT_ROOT'].'\\usersData\\'.$username.'\\'.$content['name'];
		
		function rmrdir($dirPath){
			if (substr($dirPath, strlen($dirPath) - 1, 1) != '/') {
				$dirPath .= '/';
			}
			$files = glob($dirPath . '*', GLOB_MARK);
			foreach ($files as $file) {
				if (is_dir($file)) {
					rmrdir($file);
				} else {
					unlink($file);
				}
			}
			rmdir($dirPath);
		}
		rmrdir($folder);

	}
});
?>