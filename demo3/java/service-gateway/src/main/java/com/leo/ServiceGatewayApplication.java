package com.leo;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.client.discovery.EnableDiscoveryClient;
import org.springframework.cloud.netflix.zuul.EnableZuulProxy;

@SpringBootApplication
@EnableZuulProxy
public class ServiceGatewayApplication {

	public static void main(String[] args) {
		SpringApplication.run(ServiceGatewayApplication.class, args);
	}
}
