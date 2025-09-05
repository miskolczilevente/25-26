# graph_from_file.py

def read_data(filename):
    data = []
    with open(filename, 'r') as f:
        for line in f:
            line = line.strip()
            if not line:
                continue
            parts = line.split()
            number = parts[0]
            if parts[1].startswith("(") and parts[1].endswith(")"):
                parent = parts[1][1:-1]  
                node = parts[2]
            else:
                parent = None
                node = parts[1]
            data.append((number, parent, node))
    return data

def build_graph(data):
    graph = {}
    root = None
    for _, parent, node in data:
        if parent:
            graph.setdefault(parent, []).append(node)
        else:
            root = node
    return root, graph

def print_tree(node, graph, prefix="", is_last=True, visited=None, is_root=True):
    if visited is None:
        visited = set()
    if is_root:
        print(node)
    else:
        connector = "└─ " if is_last else "├─ "
        print(f"{prefix}{connector}{node}")
    if node in visited:
        if not is_root:
            print(f"{prefix}   (loop detected!)")
        return
    visited.add(node)
    children = graph.get(node, [])
    for i, child in enumerate(children):
        is_last_child = (i == len(children) - 1)
        new_prefix = prefix + ("   " if is_last else "│  ")
        print_tree(child, graph, new_prefix, is_last_child, visited.copy(), is_root=False)



if __name__ == "__main__":
    filename = "graph_data.txt"  
    data = read_data(filename)
    root, graph = build_graph(data)
    print_tree(root, graph)
