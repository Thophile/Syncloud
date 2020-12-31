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
  $token = apache_request_headers()['Authorization'];
  
  if(isset($token)){
    
    if ($username = Token::validate($token)){
      $listFile = $_SERVER['DOCUMENT_ROOT'].'\\usersData\\'.$username.'\\list.json';

      if (file_exists($listFile)){
        $list = file_get_contents($listFile);
      }else{
        $list = "";
        file_force_contents($listFile);
      }

    }
  }
  echo $list;
});

$router->post('/list',function($request){
  $content = json_decode(file_get_contents("php://input"),true);
  $token = apache_request_headers()['Authorization'];

  if(isset($token)){

    if ($username = Token::validate($token)){

      $listFile = $_SERVER['DOCUMENT_ROOT'].'\\usersData\\'.$username.'\\list.json';

      //content is an array of name
      $remote = json_decode($content['list']);
      if(file_exists($listFile)){
        //add remote value that does not aready existe to locale list
        $locale = json_decode(file_get_contents($listFile));
        foreach ($remote as $value) {
          if(!in_array($value, $locale)){
            $locale[] = $value;
          }
        }
      }else{
        //locale is empty
        $locale = $remote;
      }
      file_force_contents($listFile, json_encode($locale));

    }
  }
})

?>