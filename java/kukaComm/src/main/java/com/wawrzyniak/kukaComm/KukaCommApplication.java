package com.wawrzyniak.kukaComm;

<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
import io.swagger.v3.oas.annotations.OpenAPIDefinition;
=======
>>>>>>> add testSocket and kukaComm
=======
import io.swagger.v3.oas.annotations.OpenAPIDefinition;
>>>>>>> add swagger docs, fix some whitespace issues
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;
@EnableScheduling
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
@OpenAPIDefinition
@SpringBootApplication
public class KukaCommApplication {

	public static void main(String[] args) {
=======
@SpringBootApplication
public class KukaCommApplication {

	public static void main(String[] args){
>>>>>>> add testSocket and kukaComm
=======
@OpenAPIDefinition
@SpringBootApplication
public class KukaCommApplication {

	public static void main(String[] args) {
>>>>>>> add swagger docs, fix some whitespace issues
		SpringApplication.run(KukaCommApplication.class, args);
	}

}
