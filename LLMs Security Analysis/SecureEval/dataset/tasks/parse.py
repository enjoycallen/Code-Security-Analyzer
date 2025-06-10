import json

def parse(path, list):
    with open(path, "r", encoding = "utf-8") as file:
        line = file.readline().strip()
        while line:
            #print(line)
            if line !=  "\n" and line.find("：") != -1:
                list.append(line)
            line = file.readline()

tasks = {
    "C" : [],
    "Cpp" : [],
    "Python" : []
}

parse("c_tasks.txt", tasks["C"])
parse("cpp_tasks.txt", tasks["Cpp"])
parse("py_tasks.txt", tasks["Python"])

with open("tasks.json", "w", encoding = "utf-8") as file:
    file.write(json.dumps(tasks, indent = 4, ensure_ascii = False))