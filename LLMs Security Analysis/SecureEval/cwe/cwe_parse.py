import xml.etree.ElementTree as ET
import json
import re

class cwe_meta:
    #加载 XML 文件
    def __init__(self, path : str, namespace : str, view_id : str):
        self.__root = ET.parse(path).getroot()
        self.__ns = {"cwe": namespace}
        self.__view_id = view_id
        self.__view = self.__root.find(f".//cwe:View[@ID='{self.__view_id}']", namespaces = self.__ns)
    
    #解析弱点
    def parse_weaknesses(self):
        self.__weaknesses = {
            "root" : {
                "id" : "root",
                "name" : self.__view.attrib["Name"],
                "description" : self.__view[0].text.strip(),
                "parents" : [],
                "peers": []
            }
        }
        
        for item in self.__root.findall(".//cwe:Weakness", namespaces = self.__ns):
            id = f"CWE-{item.attrib['ID']}"
            value = {
                "id" : id,
                "name" : item.attrib["Name"],
                "description" : re.sub(r"\s+", " ", item[0].text.strip()),
                "status" : item.attrib["Status"],
                "parents" : [],
                "peers" : [],
            }
            
            for relatedItem in item.findall(f".//cwe:Related_Weakness[@View_ID='{self.__view_id}']", namespaces = self.__ns):
                if(relatedItem.attrib["Nature"] == "ChildOf"):
                    value["parents"].append(f"CWE-{relatedItem.attrib['CWE_ID']}")
                elif(relatedItem.attrib["Nature"] == "PeerOf"):
                    value["peers"].append(f"CWE-{relatedItem.attrib['CWE_ID']}")
            
            self.__weaknesses[id] = value
        
        for member in self.__view.findall(".//cwe:Has_Member", namespaces = self.__ns):
            id = f"CWE-{member.attrib['CWE_ID']}"
            if(id in self.__weaknesses):
                self.__weaknesses[id]["parents"].append("root")

    #解析类别
    def parse_categories(self, view_id : str):
        for key in self.__weaknesses.keys():
            self.__weaknesses[key].update({"categories" : []})
        
        self.__categories = []
        for item in self.__root.findall(".//cwe:Category", namespaces = self.__ns):
            if item.attrib["Status"] == "Deprecated":
                continue
            
            id = item.attrib["ID"]
            summary = item[0].text
            self.__categories.append({
                "id" : id,
                "summary" : summary
            })
            
            for member in item.findall(f".//cwe:Has_Member[@View_ID='{view_id}']", namespaces = self.__ns):
                cwe_id = f"CWE-{member.attrib['CWE_ID']}"
                self.__weaknesses[cwe_id]["categories"].append(id)

    #生成meta    
    def finalize(self):
        self.__meta = {
            "weaknesses" : [value for value in self.__weaknesses.values()],
            "categories" : self.__categories
        }
    
    
    #保存到文件
    def save(self, path : str):
        with open(path, "w") as file:
            file.write(json.dumps(self.__meta, indent = 4))



if __name__ == "__main__":
    meta = cwe_meta("cwec_v4.17.xml", "http://cwe.mitre.org/cwe-7", "1000")
    meta.parse_weaknesses()
    meta.parse_categories("699")
    meta.finalize()
    meta.save("meta.json")