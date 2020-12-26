<?php
//Block access from file
if(__FILE__ == $_SERVER['SCRIPT_FILENAME']){
    include_once $_SERVER['DOCUMENT_ROOT'].'/errors/403.html';
    die();
}

/**
 * The class that is responsible for database connection and that stores and handle allowed queries
 * 
 * @author Thophile
 * @license MIT
 */
class Database{

    /**
     * The connection to the database
     * @var PDO|null 
     */
    private $conn = null;

    /**
     * Get the database connection
     */
    public function connect(){

        try{
            $this->conn = new PDO("mysql:host=" . env("DB_HOST") . ";port=" . env("DB_PORT") . ";dbname=" . env("DB_NAME"), env("DB_USERNAME"), env("DB_PASSWORD"));
        }catch(PDOException $exception){
            echo $exception->getMessage();
            http_response_code(500);
            include($_SERVER['DOCUMENT_ROOT'].'/errors/500.html');
            die();
        }
    }

    public function registerUser(string $username, string $mail, string $password){

        if($this->conn == null){
            $this->connect();
        }

        $query = "INSERT INTO " . env('DB_USER_TABLE') . "(username, mail, password) VALUES (:username, :mail, :password)";
        $stmt = $this->conn->prepare($query);

        $stmt->execute([
            'username' => $username,
            'mail' => $mail,
            'password' => $password
        ]);

        if($stmt->rowCount() <= 0){

            http_response_code(400);
            include($_SERVER['DOCUMENT_ROOT'].'/errors/400.html'); 
            die();
        }

    }

    public function connectUser(string $uid,string $password){

        if($this->conn == null){
            $this->connect();
        }

        $query = "SELECT * FROM ". env('DB_USER_TABLE') ." WHERE username= :uid OR mail= :uid";
        $stmt = $this->conn->prepare($query);
        if($stmt->execute(['uid' => $uid])){
            //Get record
            $user = $stmt->fetch();
        }else{
            http_response_code(400);
            die();
        }

        return password_verify($password,$user["password"]);

        
    }
}
?>