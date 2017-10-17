package com.leo.controllers;

import com.leo.models.Demo;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class DemoController {
    @Autowired
    private Demo demo;
    @RequestMapping("demo")
    public Demo demo() {
        return demo;
    }
}
