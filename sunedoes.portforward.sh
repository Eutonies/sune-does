kubectl port-forward --address=0.0.0.0 --namespace=ingress-nginx service/ingress-nginx-controller 80:80

kubectl port-forward service/sunedoes 80:80 --address=0.0.0.0

Ctrl+Z (put process on hold)
bg (run process in background)

jobs -l (list jobs)
fg <job-number> (bring process to foreground)
Ctr+C
