package com.wawrzyniak.testsocket;

import io.swagger.v3.oas.annotations.OpenAPIDefinition;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;

@EnableScheduling
@SpringBootApplication
@OpenAPIDefinition
public class TestsocketApplication {

	public static void main(String[] args) {
		SpringApplication.run(TestsocketApplication.class, args);
	}

}
